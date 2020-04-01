using AutoMapper;
using Ecommerce.Backend.Common.DTO;
using Ecommerce.Backend.Entities;

namespace Ecommerce.Backend.Common.AutoMappingProfiles
{
  public class PaymentMethodMappingProfile : Profile
  {
    public PaymentMethodMappingProfile()
    {
      CreateMap<PaymentMethodAddDto, PaymentMethod>();
    }
  }
}
