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
using MongoDB.Driver;
using MongoDB.Entities;

namespace Ecommerce.Backend.Services.Implementations
{
  public class CartService : ICartService
  {
    private readonly IMongoCollection<Cart> _carts;
    private readonly IProductService _productService;

    public CartService(IProductService productService)
    {
      _carts = DB.Collection<Cart>();
      _productService = productService;
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
      if (cartId == null && customerId == null) return null;
      Expression<Func<Cart, bool>> cartIdFilter = (c) => c.ID == cartId;
      Expression<Func<Cart, bool>> cartCustomerIdFilter = (c) => c.CustomerId == customerId && c.Status == CartStatus.Active;
      var filterConditions = Builders<Cart>.Filter.Where(cartId != null ? cartIdFilter : cartCustomerIdFilter);
      var cart = await _carts.FindAsync<Cart>(filterConditions).Result.FirstOrDefaultAsync();
      cart.Products.ForEach(async(p) =>
      {
        var product = await _productService.GetProductById(p.ProductId);
        p.FeatureImageUrl = product.FeatureImageUrl;
      });
      return cart;
    }

    /// <summary>
    /// Add cart is not suitable to directly use
    /// </summary>
    public async Task<Cart> AddCart(Cart cart)
    {
      cart.Quantity = cart.Products.Sum(s => s.Quantity);
      cart.Products.ForEach(cartProduct =>
      {
        cartProduct.TotalPrice = cartProduct.UnitPrice * cartProduct.Quantity;
        cart.TotalPrice += cartProduct.TotalPrice;
      });
      await _carts.InsertOneAsync(cart);
      cart.Products.ForEach(async(p) =>
      {
        var product = await _productService.GetProductById(p.ProductId);
        p.FeatureImageUrl = product.FeatureImageUrl;
      });
      return cart;
    }

    public async Task<Cart> AddCartProduct(AddCartProductDto dto)
    {
      var product = await _productService.GetProductById(dto.ProductId);
      if (product == null) return null;
      var productColor = product.ProductColors.FirstOrDefault(x => x.ColorCode.ToLower() == dto.Color.ToLower());
      if (productColor == null) return null;
      if (!productColor.Sizes.Any(productSize => productSize == dto.Size)) return null;
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

      if (dto.CustomerId == "") dto.CustomerId = null;

      var cart = new Cart
      {
        CustomerId = dto.CustomerId,
        Products = new List<CartProduct> { cartProduct },
        Status = CartStatus.Active
      };
      var createdCart = await AddCart(cart);
      return createdCart;
    }

    public async Task<Cart> UpdateCartById(string cartId, Cart cart)
    {
      cart.Quantity = cart.Products.Sum(s => s.Quantity);
      cart.Products.ForEach(cartProduct =>
      {
        cartProduct.TotalPrice = cartProduct.UnitPrice * cartProduct.Quantity;
        cart.TotalPrice += cartProduct.TotalPrice;
      });
      cart.UpdatedAt = DateTime.Now;

      var update = Builders<Cart>.Update
        .Set("Quantity", cart.Quantity)
        .Set("TotalPrice", cart.TotalPrice)
        .Set("UpdatedAt", cart.UpdatedAt)
        .Set("Products", cart.Products);
      var options = new FindOneAndUpdateOptions<Cart, Cart> { ReturnDocument = ReturnDocument.After };
      var updatedCart = await _carts.FindOneAndUpdateAsync<Cart, Cart>(r => r.ID == cartId, update, options);
      return updatedCart;
    }
  }
}