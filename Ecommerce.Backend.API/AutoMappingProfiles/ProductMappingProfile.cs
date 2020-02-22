using AutoMapper;
using Ecommerce.Backend.Common.DTO;
using Ecommerce.Backend.Entities;

namespace Ecommerce.Backend.API.AutoMappingProfiles
{
  public class ProductMappingProfile : Profile
  {
    public ProductMappingProfile()
    {
      CreateMap<ProductAddDto, Product>()
        .ForMember(product => product.ManufactureDetail, opt => opt.MapFrom(dto => dto.ManufactureDetail))
        .ForMember(product => product.Pricing, opt => opt.MapFrom(dto => dto.Pricing))
        .ForMember(product => product.ShippingDetail, opt => opt.MapFrom(dto => dto.ShippingDetail));
      CreateMap<PricingDto, Pricing>();
      CreateMap<ShippingDetailDto, ShippingDetail>();
      CreateMap<ManufactureDetailDto, ManufactureDetail>();
    }
  }
}