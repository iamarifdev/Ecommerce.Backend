using System.Collections.Generic;
using Ecommerce.Backend.Common.DTO;
using FluentValidation;

namespace Ecommerce.Backend.API.Validators
{
  public class OrderAddValidator : AbstractValidator<OrderAddDto>
  {
    private readonly List<string> _allowedCurrencies = new List<string>
    {
      "BDT", "USD", "EUR", "SGD", "INR", "MYR"
    };

    public OrderAddValidator()
    {
      RuleFor(r => r.CustomerId).NotNull();
      RuleFor(r => r.ShippingMethodId).NotEmpty();
      RuleFor(r => r.PaymentMethodId).NotEmpty();
      RuleFor(r => r.Currency).NotEmpty().Must(currency => _allowedCurrencies.Contains(currency));
    }
  }
}