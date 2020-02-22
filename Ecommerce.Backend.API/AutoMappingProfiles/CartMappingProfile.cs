using AutoMapper;
using Ecommerce.Backend.Common.DTO;
using Ecommerce.Backend.Entities;

namespace Ecommerce.Backend.API.AutoMappingProfiles
{
  public class CartMappingProfile : Profile
  {
    public CartMappingProfile()
    {
      CreateMap<CartDto, Cart>()
        .ForMember(cart => cart.Products, opt => opt.MapFrom(dto => dto.Products));
      CreateMap<CartProductDto, CartProduct>();
    }
  }
}