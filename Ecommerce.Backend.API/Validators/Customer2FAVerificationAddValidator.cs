using Ecommerce.Backend.Common.DTO;
using FluentValidation;

namespace Ecommerce.Backend.API.Validators
{
  public class Customer2FAVerificationAddValidator : AbstractValidator<Customer2FAVerificationAddDto>
  {
    public Customer2FAVerificationAddValidator()
    {
      RuleFor(r => r.PhoneNo).NotEmpty().Matches(@"^01[3456789][0-9]{8}$");
    }
  }
}
