using System.Threading.Tasks;
using Ecommerce.Backend.Common.DTO;
using Ecommerce.Backend.Common.Models;
using Ecommerce.Backend.Entities;
using Ecommerce.Backend.Services.Abstractions;

namespace Ecommerce.Backend.Services.Implementations
{
  public class CustomerService : BaseService<Customer>, ICustomerService
  {
    public async Task<PagedList<CustomerListItemDto>> GetPaginatedCustomerList(PagedQuery query)
    {
      var items = await GetPaginatedList<CustomerListItemDto>(query, customer => new CustomerListItemDto
      {
        FirstName = customer.FirstName,
          LastName = customer.LastName,
          AvatarUrl = customer.AvatarUrl,
          Email = customer.Email,
          PhoneNo = customer.PhoneNo,
          ProfileCompleteness = customer.ProfileCompleteness
      });
      return items;
    }
  }
}