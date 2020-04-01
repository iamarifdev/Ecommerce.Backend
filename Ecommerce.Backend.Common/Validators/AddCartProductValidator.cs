using Ecommerce.Backend.Common.DTO;
using FluentValidation;

namespace Ecommerce.Backend.Common.Validators
{
  public class AddCartProductValidator : AbstractValidator<AddCartProductDto>
  {
    public AddCartProductValidator()
    {
      RuleFor(r => r.ProductId).NotEmpty();
      RuleFor(r => r.Quantity).GreaterThan(0);
      RuleFor(r => r.Color).NotEmpty().Matches(@"^#([A-Fa-f0-9]{3}){1,2}\b$");
      RuleFor(r => r.Size).GreaterThan(0);
      RuleFor(r => r.CustomerId).NotNull().When(w => w.CustomerId != null);
      RuleFor(r => r.CartId).NotNull().When(w => w.CartId != null);
    }
  }
}