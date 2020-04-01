using Ecommerce.Backend.Common.DTO;
using FluentValidation;

namespace Ecommerce.Backend.Common.Validators
{
  public class ShippingMethodAddValidator : AbstractValidator<ShippingMethodAddDto>
  {
    public ShippingMethodAddValidator()
    {
      RuleFor(r => r.Cost).NotEmpty().GreaterThanOrEqualTo(0);
      RuleFor(r => r.IsOutSide).NotNull();
      RuleFor(r => r.MethodName).NotEmpty();
      RuleFor(r => r.IsEnabled).NotNull();
    }
  }
}
