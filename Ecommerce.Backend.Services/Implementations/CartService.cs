using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Ecommerce.Backend.Common.Helpers;
using Ecommerce.Backend.Common.Models;
using Ecommerce.Backend.Entities;
using Ecommerce.Backend.Services.Abstractions;
using MongoDB.Bson;
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
      return cart;
    }

    public async Task<Cart> AddCartProduct(string productId, double quantity, string color, double size, string customerId = "")
    {
      // todo change
      var product = await _productService.GetProductById(productId);
      if (product == null) return null;
      var productColor = product.ProductColors.FirstOrDefault(x => x.Color == color);
      if (productColor == null) return null;
      if (!productColor.Sizes.Any(productSize => productSize == size)) return null;
      var cartProduct = new CartProduct
      {
        ProductId = product.ID,
        Quantity = quantity,
        SKU = product.SKU,
        Title = product.Title,
        UnitPrice = productColor?.Pricing.Price ?? 0,
      };

      if (customerId == "") customerId = null;

      var cart = new Cart
      {
        CustomerId = customerId,
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