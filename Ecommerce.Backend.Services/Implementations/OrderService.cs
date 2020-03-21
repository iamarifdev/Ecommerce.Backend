using System.Threading.Tasks;
using Ecommerce.Backend.Common.DTO;
using Ecommerce.Backend.Common.Models;
using Ecommerce.Backend.Entities;
using Ecommerce.Backend.Services.Abstractions;

namespace Ecommerce.Backend.Services.Implementations
{
  public class OrderService : BaseService<Order>, IOrderService
  {
    public async Task<PagedList<OrderListItemDto>> GetPaginatedOrderList(PagedQuery query)
    {
      var items = await GetPaginatedList<OrderListItemDto>(query, order => new OrderListItemDto(order));
      return items;
    }
  }
}