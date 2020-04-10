using Ecommerce.Backend.Common.DTO;
using FluentValidation;

namespace Ecommerce.Backend.API.Validators
{
  public class CustomerLoginValidator : AbstractValidator<CustomerLoginDto>
  {
    public CustomerLoginValidator()
    {
      RuleFor(r => r.PhoneNo).NotEmpty();
      RuleFor(r => r.Password).NotEmpty();
    }
  }
}