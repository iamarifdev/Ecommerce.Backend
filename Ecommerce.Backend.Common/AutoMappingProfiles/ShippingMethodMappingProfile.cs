using AutoMapper;
using Ecommerce.Backend.Common.DTO;
using Ecommerce.Backend.Entities;

namespace Ecommerce.Backend.Common.AutoMappingProfiles
{
  public class ShippingMethodMappingProfile : Profile
  {
    public ShippingMethodMappingProfile()
    {
      CreateMap<ShippingMethodAddDto, ShippingMethod>();
    }
  }
}