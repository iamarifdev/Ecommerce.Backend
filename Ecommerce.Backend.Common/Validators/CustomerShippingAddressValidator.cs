using Ecommerce.Backend.Common.DTO;
using FluentValidation;

namespace Ecommerce.Backend.Common.Validators
{
  public class CustomerShippingAddressValidator : AbstractValidator<CustomerShippingAddressDto>
  {
    public CustomerShippingAddressValidator()
    {
      RuleFor(r => r.SameToBillingAddress).NotNull();
      When(address => !address.SameToBillingAddress, () =>
      {
        RuleFor(r => r.PhoneNo).NotEmpty().Matches(@"^01[3456789][0-9]{8}$");
        RuleFor(r => r.Email).EmailAddress();
        RuleFor(r => r.FirstName).NotEmpty();
        RuleFor(r => r.LastName).NotEmpty();
        RuleFor(r => r.Country).NotEmpty();
        RuleFor(r => r.State).NotEmpty();
        RuleFor(r => r.City).NotEmpty();
        RuleFor(r => r.PostalCode).NotEmpty();
      });
    }
  }
}