using Ecommerce.Backend.Common.DTO;
using FluentValidation;

namespace Ecommerce.Backend.API.Validators
{
  public class ProductUpdateValidator : AbstractValidator<ProductUpdateDto>
  {
    public ProductUpdateValidator()
    {
      RuleFor(r => r.SKU).NotEmpty();
      RuleFor(r => r.Title).NotEmpty();
      RuleFor(r => r.Description).NotEmpty();
      RuleFor(r => r.ManufactureDetail).NotEmpty();
      RuleFor(r => r.ManufactureDetail.ModelNo).NotEmpty().When(w => w.ManufactureDetail != null);
      RuleFor(r => r.Pricing).NotEmpty();
      RuleFor(r => r.Pricing.Price).NotEmpty().When(w => w.Pricing != null).GreaterThan(0);
      RuleFor(r => r.ProductColors).NotEmpty();
      RuleFor(r => r.FeatureImageUrl).NotEmpty().When(w => w.FeatureImageUrl != null);
      RuleForEach(rf => rf.ProductColors).SetValidator(new ProductColorValidator());
    }
  }
}
