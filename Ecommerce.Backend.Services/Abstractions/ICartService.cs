using System.Threading.Tasks;
using Ecommerce.Backend.Entities;

namespace Ecommerce.Backend.Services.Abstractions
{
  public interface ICartService
  {
    Task<Cart> GetCartById(string cartId = null, string customerId = null);
    Task<Cart> UpdateCartById(string cartId, Cart cart);
    Task<Cart> AddCart(Cart cart);
  }
}