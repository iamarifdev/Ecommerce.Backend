using Ecommerce.Backend.Common.DTO;
using FluentValidation;

namespace Ecommerce.Backend.Common.Validators
{
  public class UpdateCartProductValidator : AbstractValidator<UpdateCartProductDto>
  {
    public UpdateCartProductValidator()
    {
      RuleFor(r => r.CartProductId).NotEmpty();
      RuleFor(r => r.ProductId).NotEmpty();
      RuleFor(r => r.Quantity).NotEmpty().GreaterThan(0);
    }
  }
}