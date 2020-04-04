using Ecommerce.Backend.Common.DTO;
using FluentValidation;

namespace Ecommerce.Backend.API.Validators
{
  public class UserAddValidator : AbstractValidator<UserAddDto>
  {
    public UserAddValidator()
    {
      RuleFor(r => r.FullName).NotEmpty();
      RuleFor(r => r.Username).NotEmpty();
      RuleFor(r => r.Email).NotEmpty().EmailAddress();
      RuleFor(r => r.RoleId).NotEmpty();
      RuleFor(r => r.Addresses).NotEmpty().NotNull();
      RuleFor(r => r.PhoneNumbers).NotEmpty().NotNull();
    }
  }
}