using Ecommerce.Backend.Common.DTO;
using FluentValidation;

namespace Ecommerce.Backend.API.Validators
{
  public class RoleAddValidator : AbstractValidator<RoleAddDto>
  {
    public RoleAddValidator()
    {
      RuleFor(r => r.Name).NotEmpty();
      RuleFor(r => r.Description).NotEmpty();
      RuleFor(r => r.IsEnabled).NotNull();
    }
  }
}