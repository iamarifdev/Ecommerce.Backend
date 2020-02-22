using Ecommerce.Backend.Common.DTO;
using FluentValidation;

namespace Ecommerce.Backend.API.Validators
{
  public class ProductAddValidator : AbstractValidator<ProductAddDto>
  {
    public ProductAddValidator()
    {
      RuleFor(r => r.SKU).NotEmpty();
      RuleFor(r => r.InStock).GreaterThanOrEqualTo(0);
      RuleFor(r => r.Availibility).NotEmpty();
      RuleFor(r => r.Title).NotEmpty();
      RuleFor(r => r.Description).NotEmpty();
      RuleFor(r => r.ManufactureDetail).NotEmpty();
      RuleFor(r => r.ManufactureDetail.ModelNo).NotEmpty();
      RuleFor(r => r.ShippingDetail).NotEmpty();
      RuleFor(r => r.ShippingDetail.Sizes).NotEmpty().ForEach(f => f.GreaterThan(0));
      RuleFor(r => r.Pricing).NotEmpty();
      RuleFor(r => r.Pricing.Price).NotEmpty().GreaterThan(0);
      RuleFor(r => r.Colors).NotEmpty().ForEach(f => f.Matches(@"^#([a-f0-9]{3}){1,2}\b$"));
    }
  }
}