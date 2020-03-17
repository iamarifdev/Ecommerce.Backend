using Ecommerce.Backend.Common.DTO;
using FluentValidation;

namespace Ecommerce.Backend.API.Validators
{
  public class PaymentMethodAddValidator : AbstractValidator<PaymentMethodAddDto>
  {
    public PaymentMethodAddValidator()
    {
      RuleFor(r => r.HasPaymentGateway).NotNull();
      RuleFor(r => r.MethodName).NotEmpty();
      RuleFor(r => r.IsEnabled).NotNull();
    }
  }
}
