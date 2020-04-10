using Ecommerce.Backend.Common.DTO;
using FluentValidation;

namespace Ecommerce.Backend.API.Validators
{
  public class CustomerAddValidator : AbstractValidator<CustomerAddDto>
  {
    public CustomerAddValidator()
    {
      RuleFor(r => r.PhoneNo).NotEmpty().Matches(@"^01[3456789][0-9]{8}$");
      RuleFor(r => r.FullName).NotEmpty().MinimumLength(3);
      RuleFor(r => r.Password).NotEmpty().Length(6, 32);
      RuleFor(r => r.VerificationCode).NotEmpty().Length(6,6);
      RuleFor(r => r.Email).EmailAddress();
    }
  }
}
