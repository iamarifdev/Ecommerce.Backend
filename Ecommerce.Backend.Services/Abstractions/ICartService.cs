using System.Threading.Tasks;
using Ecommerce.Backend.Common.DTO;
using Ecommerce.Backend.Common.Models;
using Ecommerce.Backend.Entities;

namespace Ecommerce.Backend.Services.Abstractions
{
  public interface ICartService
  {
    Task<PagedList<Cart>> GetPaginatedCarts(PagedQuery query);
    Task<Cart> GetCartById(string cartId = null, string customerId = null);
    Task<Cart> UpdateCartById(string cartId, Cart cart);

    /// <summary>
    /// Add cart is not suitable
    /// </summary>
    Task<Cart> AddCart(Cart cart);
    Task<Cart> AddCartProduct(AddCartProductDto dto);
  }
}