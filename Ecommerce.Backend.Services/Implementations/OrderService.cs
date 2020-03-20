using System.Threading.Tasks;
using Ecommerce.Backend.Common.DTO;
using Ecommerce.Backend.Common.Models;
using Ecommerce.Backend.Entities;
using Ecommerce.Backend.Services.Abstractions;

namespace Ecommerce.Backend.Services.Implementations
{
  public class OrderService : BaseService<Order>, IOrderService
  {
    public async Task<PagedList<Order>> GetPaginatedOrderList(PagedQuery query)
    {
      var items = await GetPaginatedList<Order>(query, order => new Order(order.ID, order.Customer));
      return items;
    }
  }
}