using Ecommerce.Backend.API.DTO;
using FluentValidation;

namespace Ecommerce.Backend.API.Validators
{
  public class AddCartProductValidator : AbstractValidator<AddCartProductDto>
  {
    public AddCartProductValidator()
    {
      RuleFor(r => r.ProductId).NotEmpty();
      RuleFor(r => r.Quantity).NotEqual(0);
      RuleFor(r => r.Color).NotEmpty().MaximumLength(10);
      RuleFor(r => r.Size).NotEqual(0);
      RuleFor(r => r.CustomerId).NotNull().When(w => w.CustomerId != null);
    }
  }
}