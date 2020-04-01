using AutoMapper;
using Ecommerce.Backend.Common.DTO;
using Ecommerce.Backend.Entities;

namespace Ecommerce.Backend.Common.AutoMappingProfiles
{
  public class ProductMappingProfile : Profile
  {
    public ProductMappingProfile()
    {
      CreateMap<ProductAddDto, Product>()
        .ForMember(product => product.ManufactureDetail, opt => opt.MapFrom(dto => dto.ManufactureDetail))
        .ForMember(product => product.ProductColors, opt => opt.MapFrom(dto => dto.ProductColors));
      CreateMap<ProductUpdateDto, Product>()
        .ForMember(product => product.ManufactureDetail, opt => opt.MapFrom(dto => dto.ManufactureDetail))
        .ForMember(product => product.ProductColors, opt => opt.MapFrom(dto => dto.ProductColors));
      CreateMap<PricingDto, Pricing>();
      CreateMap<ProductColorDto, ProductColor>();
      CreateMap<ManufactureDetailDto, ManufactureDetail>();
    }
  }
}