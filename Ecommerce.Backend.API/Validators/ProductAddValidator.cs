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
      RuleFor(r => r.ManufactureDetail.ModelNo).NotEmpty().When(w => w.ManufactureDetail != null);
      RuleFor(r => r.ProductColors).NotEmpty();
      RuleForEach(rf => rf.ProductColors).SetValidator(new ProductColorValidator());
    }
  }
}