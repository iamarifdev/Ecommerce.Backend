using Ecommerce.Backend.Common.DTO;
using FluentValidation;

namespace Ecommerce.Backend.API.Validators
{
  public class CustomerAddValidator : AbstractValidator<CustomerAddDto>
  {
    public CustomerAddValidator()
    {
      RuleFor(r => r.PhoneNo)
        .NotEmpty()
        .When(w => string.IsNullOrWhiteSpace(w.Email))
        .Matches(@"^01[3456789][0-9]{8}$");
      RuleFor(r => r.Email)
        .NotEmpty()
        .When(w => string.IsNullOrWhiteSpace(w.PhoneNo))
        .EmailAddress();
    }
  }
}
