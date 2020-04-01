using Ecommerce.Backend.Common.DTO;
using FluentValidation;

namespace Ecommerce.Backend.Common.Validators
{
  public class ProductColorValidator : AbstractValidator<ProductColorDto>
  {
    public ProductColorValidator()
    {
      RuleFor(r => r.ColorCode).NotEmpty().Matches(@"^#([A-Fa-f0-9]{3}){1,2}\b$");
      RuleFor(r => r.ColorName).NotEmpty();
      RuleFor(r => r.InStock).NotEmpty().GreaterThanOrEqualTo(0);
      RuleFor(r => r.IsAvailable).NotEmpty();
      RuleFor(r => r.Sizes).NotEmpty().When(w => w.Sizes != null);
    }
  }
}