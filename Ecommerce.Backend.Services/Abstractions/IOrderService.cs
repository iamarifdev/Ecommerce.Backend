using System.Threading.Tasks;
using Ecommerce.Backend.Common.DTO;
using Ecommerce.Backend.Common.Models;
using Ecommerce.Backend.Entities;

namespace Ecommerce.Backend.Services.Abstractions
{
  public interface IOrderService : IBaseService<Order>
  {
    Task<PagedList<Order>> GetPaginatedOrderList(PagedQuery query);
  }
}