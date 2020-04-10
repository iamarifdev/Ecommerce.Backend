using System.Threading.Tasks;
using Ecommerce.Backend.Common.DTO;
using Ecommerce.Backend.Common.Helpers;
using Ecommerce.Backend.Common.Models;
using Ecommerce.Backend.Entities;
using Ecommerce.Backend.Services.Abstractions;

namespace Ecommerce.Backend.Services.Implementations
{
  public class CustomerService : BaseService<Customer>, ICustomerService
  {
    public async Task<Customer> AddCustomer(Customer customer)
    {
      var salt = Salt.Create();
      customer.Auth.Password = Hash.Create(customer.Auth.Password, salt);
      customer.Auth.Salt = salt;
      await Add(customer);
      return customer;
    }

    public async Task<PagedList<CustomerListItemDto>> GetPaginatedCustomerList(PagedQuery query)
    {
      var items = await GetPaginatedList<CustomerListItemDto>(query, customer => new CustomerListItemDto
      {
        ID = customer.ID,
          FullName = customer.FullName,
          AvatarUrl = customer.AvatarUrl,
          Email = customer.Email,
          PhoneNo = customer.PhoneNo,
          ProfileCompleteness = customer.ProfileCompleteness
      });
      return items;
    }

    public async Task<Customer> UpdateBillingAddress(string customerId, BillingAddress address)
    {
      var customer = await GetById(customerId);
      customer.BillingAddress = address;
      var updatedCustomer = await UpdateById(customerId, customer);
      return updatedCustomer;
    }

    public async Task<Customer> UpdateShippingAddress(string customerId, ShippingAddress address)
    {
      var customer = await GetById(customerId);
      if (address.SameToBillingAddress)
      {
        customer.ShippingAddress = new ShippingAddress
        {
          FullName = customer.BillingAddress.FullName,
          Email = customer.BillingAddress.Email,
          PhoneNo = customer.BillingAddress.PhoneNo,
          Country = customer.BillingAddress.Country,
          State = customer.BillingAddress.State,
          City = customer.BillingAddress.City,
          Address = customer.BillingAddress.Address,
          PostalCode = customer.BillingAddress.PostalCode,
          SameToBillingAddress = true
        };
      }
      else
      {
        customer.ShippingAddress = address;
      }
      var updatedCustomer = await UpdateById(customerId, customer);
      return updatedCustomer;
    }
  }
}