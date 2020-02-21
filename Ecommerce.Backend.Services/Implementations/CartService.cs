using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Ecommerce.Backend.Entities;
using Ecommerce.Backend.Services.Abstractions;
using MongoDB.Driver;
using MongoDB.Entities;

namespace Ecommerce.Backend.Services.Implementations
{
  public class CartService : ICartService
  {
    private readonly IMongoCollection<Cart> _carts;

    public CartService()
    {
      _carts = DB.Collection<Cart>();
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

    public async Task<Cart> AddCart(Cart cart)
    {
      // TODO: associate customer id, if the customer is logged in
      cart.Quantity = cart.Products.Sum(s => s.Quantity);
      cart.Total = cart.Products.Sum(s => s.Price);
      await _carts.InsertOneAsync(cart);
      return cart;
    }

    public async Task<Cart> UpdateCartById(string cartId, Cart cart)
    {
      cart.Quantity = cart.Products.Sum(s => s.Quantity);
      cart.Total = cart.Products.Sum(s => s.Price);
      cart.UpdatedAt = DateTime.Now;

      var update = Builders<Cart>.Update
        .Set("Quantity", cart.Quantity)
        .Set("Total", cart.Total)
        .Set("UpdatedAt", cart.UpdatedAt)
        .Set("Products", cart.Products);
      var updatedCart = await _carts.FindOneAndUpdateAsync(r => r.ID == cartId, update);
      return updatedCart;
    }
  }
}