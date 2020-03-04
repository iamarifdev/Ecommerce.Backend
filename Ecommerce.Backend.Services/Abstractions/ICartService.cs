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
    Task<Cart> AddCartProduct(AddCartProductDto dto);
    Task<Cart> UpdateProductQuantity(string cartId, UpdateCartProductDto dto);
    Task<Cart> RemoveCartProduct(string cartId, string cartProductId);
  }
}