using Ecommerce.Backend.Common.DTO;
using FluentValidation;

namespace Ecommerce.Backend.API.Validators
{
  public class LoginValidator : AbstractValidator<LoginDto>
  {
    public LoginValidator()
    {
      RuleFor(r => r.Username).NotEmpty();
      RuleFor(r => r.Password).NotEmpty();
    }
  }
}