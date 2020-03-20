namespace Ecommerce.Backend.Common.DTO
{
  public class CustomerShippingAddressDto : CustomerBillingAddressDto
  {
    public bool SameToBillingAddress { get; set; }
  }
}
