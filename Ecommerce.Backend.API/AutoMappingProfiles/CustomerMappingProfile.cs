using AutoMapper;
using Ecommerce.Backend.Common.DTO;
using Ecommerce.Backend.Entities;

namespace Ecommerce.Backend.API.AutoMappingProfiles
{
  public class CustomerMappingProfile : Profile
  {
    public CustomerMappingProfile()
    {
      CreateMap<CustomerAddDto, Customer>().ForPath(m => m.Auth.Password, (opt) => opt.MapFrom(dest => dest.Password));
      CreateMap<CustomerBillingAddressDto, BillingAddress>();
      CreateMap<CustomerShippingAddressDto, ShippingAddress>();
    }
  }
}
