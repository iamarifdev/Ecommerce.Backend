using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Ecommerce.Backend.Common.DTO;
using Ecommerce.Backend.Common.Helpers;
using Ecommerce.Backend.Common.Models;
using Ecommerce.Backend.Entities;
using Ecommerce.Backend.Services.Abstractions;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Entities;

namespace Ecommerce.Backend.Services.Implementations
{
  public class CartService : BaseService<Cart>, ICartService
  {
    private readonly IMongoCollection<Cart> _carts;
    private readonly IProductService _productService;

    public CartService(IProductService productService)
    {
      _carts = DB.Collection<Cart>();
      _productService = productService;
    }

    private async Task<Cart> _populateCartProductsImage(Cart cart)
    {
      foreach (var cartProduct in cart.Products)
      {
        var product = await _productService.GetProductById(cartProduct.ProductId);
        cartProduct.FeatureImageUrl = product.FeatureImageUrl;
      }
      return cart;
    }

    private async Task<Cart> _addCart(Cart cart)
    {
      cart.Quantity = cart.Products.Sum(s => s.Quantity);
      cart.Products.ForEach(cartProduct =>
      {
        cartProduct.TotalPrice = cartProduct.UnitPrice * cartProduct.Quantity;
        cart.TotalPrice += cartProduct.TotalPrice;
      });
      await _carts.InsertOneAsync(cart);
      cart = await _populateCartProductsImage(cart);
      return cart;
    }

    private async Task<Cart> _updateCartById(string cartId, Cart cart)
    {
      cart.Products.ForEach(cartProduct =>
      {
        cartProduct.TotalPrice = cartProduct.UnitPrice * cartProduct.Quantity;
      });
      cart.TotalPrice = cart.Products.Sum(s => s.TotalPrice);
      cart.Quantity = cart.Products.Sum(s => s.Quantity);
      cart.UpdatedAt = DateTime.Now;

      var update = Builders<Cart>.Update
        .Set("Quantity", cart.Quantity)
        .Set("TotalPrice", cart.TotalPrice)
        .Set("UpdatedAt", cart.UpdatedAt)
        .Set("Products", cart.Products);
      var options = new FindOneAndUpdateOptions<Cart, Cart> { ReturnDocument = ReturnDocument.After };
      var updatedCart = await _carts.FindOneAndUpdateAsync<Cart, Cart>(r => r.ID == cartId, update, options);
      updatedCart = await _populateCartProductsImage(updatedCart);
      return updatedCart;
    }

    private CartProduct _prepareCartProduct(Product product, AddCartProductDto dto)
    {
      var cartProduct = new CartProduct
      {
        ProductId = product.ID,
        Quantity = dto.Quantity,
        SKU = product.SKU,
        Title = product.Title,
        UnitPrice = product?.Pricing.Price ?? 0,
        Size = dto.Size,
        Color = dto.Color
      };
      return cartProduct;
    }

    private async Task<Cart> _addProductToExistingCart(AddCartProductDto dto)
    {
      var product = await _productService.GetProductById(dto.ProductId);
      var cart = await GetCartById(dto.CartId);
      var cartProduct = _prepareCartProduct(product, dto);
      cart.Products.Add(cartProduct);
      if (dto.CustomerId == "") dto.CustomerId = null;
      cart.CustomerId = dto.CustomerId;
      var updatedCart = await _updateCartById(cart.ID, cart);
      updatedCart = await _populateCartProductsImage(updatedCart);
      return updatedCart;
    }

    public async Task<PagedList<Cart>> GetPaginatedCarts(PagedQuery query)
    {
      Expression<Func<Cart, bool>> allConditions = (_) => true;
      Expression<Func<Cart, bool>> conditions = (cart) => cart.Status == CartStatus.Active;
      var filterConditions = Builders<Cart>.Filter.Where(query.All ? allConditions : conditions);

      var count = (int) await _carts.CountDocumentsAsync(filterConditions);
      var carts = await _carts
        .Find(filterConditions)
        .Project(cart => new Cart
        {
          ID = cart.ID,
            Status = cart.Status,
            CustomerId = cart.CustomerId,
            Products = cart.Products,
            Quantity = cart.Quantity,
            TotalPrice = cart.TotalPrice,
            CreatedAt = cart.CreatedAt,
            UpdatedAt = cart.UpdatedAt
        })
        .SortBy(cart => cart.UpdatedAt)
        .Skip(query.PageSize * (query.Page - 1))
        .Limit(query.PageSize)
        .ToListAsync();
      return carts.ToPagedList(count);
    }

    public async Task<Cart> GetCartById(string cartId = null, string customerId = null)
    {
      if (cartId.IsEmpty() && customerId.IsEmpty()) return null;
      Expression<Func<Cart, bool>> cartIdFilter = (c) => c.ID == cartId;
      Expression<Func<Cart, bool>> cartCustomerIdFilter = (c) => c.CustomerId == customerId && c.Status == CartStatus.Active;
      var filterConditions = Builders<Cart>.Filter.Where(cartId != null ? cartIdFilter : cartCustomerIdFilter);
      var cart = await _carts.FindAsync<Cart>(filterConditions).Result.FirstOrDefaultAsync();
      if (cart == null) return null;
      cart = await _populateCartProductsImage(cart);
      return cart;
    }

    public async Task<Cart> AssignCustomerId(string cartId, string customerId)
    {
      if (cartId.IsEmpty() || customerId.IsEmpty()) return null;
      var update = Builders<Cart>.Update.Set(cart => cart.CustomerId, customerId);
      var updatedCart = await UpdatePartial(cart => cart.ID == cartId, update);
      updatedCart = await _populateCartProductsImage(updatedCart);   
      return updatedCart;
    }

    public async Task<Cart> AddCartProduct(AddCartProductDto dto)
    {
      if (!string.IsNullOrWhiteSpace(dto.CartId))
      {
        return await _addProductToExistingCart(dto);
      }
      var product = await _productService.GetProductById(dto.ProductId);
      if (product == null) return null;
      var productColor = product.ProductColors.FirstOrDefault(x => x.ColorCode.ToLower() == dto.Color.ToLower());
      if (productColor == null) return null;
      if (!productColor.Sizes.Any(productSize => productSize == dto.Size)) return null;
      var cartProduct = _prepareCartProduct(product, dto);

      if (dto.CustomerId == "") dto.CustomerId = null;

      var cart = new Cart
      {
        CustomerId = dto.CustomerId,
        Products = new List<CartProduct> { cartProduct },
        Status = CartStatus.Active
      };
      var createdCart = await _addCart(cart);
      return createdCart;
    }

    public async Task<Cart> UpdateProductQuantity(string cartId, UpdateCartProductDto dto)
    {
      var cart = await GetCartById(cartId);
      if (cart == null) return null;
      var product = await _productService.GetProductById(dto.ProductId);
      if (product == null) return null;
      var isProductExistInCart = cart.Products.Any(p => p.ProductId == dto.ProductId);
      if (!isProductExistInCart) return cart;

      var cartProduct = cart.Products.FirstOrDefault(cp => cp.ProductId == dto.ProductId && cp.ID == dto.CartProductId);
      if (cartProduct == null) return cart;
      cartProduct.Quantity = dto.Quantity;
      cartProduct.UnitPrice = product.Pricing.Price;
      cartProduct.TotalPrice = cartProduct.UnitPrice * dto.Quantity;
      cart.Quantity = cart.Products.Sum(s => s.Quantity);
      cart.TotalPrice = cart.Products.Sum(s => s.TotalPrice);

      var update = Builders<Cart>.Update
        .Set("Quantity", cart.Quantity)
        .Set("TotalPrice", cart.TotalPrice)
        .Set("UpdatedAt", cart.UpdatedAt)
        .Set("Products", cart.Products);
      var options = new FindOneAndUpdateOptions<Cart, Cart> { ReturnDocument = ReturnDocument.After };
      var updatedCart = await _carts.FindOneAndUpdateAsync<Cart, Cart>(r => r.ID == cartId, update, options);
      updatedCart = await _populateCartProductsImage(updatedCart);
      return updatedCart;
    }

    public async Task<Cart> RemoveCartProduct(string cartId, string cartProductId)
    {
      var filterDef = Builders<Cart>.Filter;
      var updateDef = Builders<Cart>.Update;

      var condition = filterDef.Eq(x => x.ID, cartId);
      var update = updateDef.PullFilter(p => p.Products, f => f.ID == cartProductId);
      var options = new FindOneAndUpdateOptions<Cart, Cart> { ReturnDocument = ReturnDocument.After };
      var updatedCart = await _carts.FindOneAndUpdateAsync(condition, update, options);
      updatedCart = await _populateCartProductsImage(updatedCart);
      return updatedCart;
    }
  }
}