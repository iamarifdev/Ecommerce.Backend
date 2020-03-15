namespace Ecommerce.Backend.Entities
{
  public class Customer : BaseEntity
  {
    public BillingAddress BillingAddress { get; set; }
    public ShippingAddress ShippingAddress { get; set; }
  }

  public class BillingAddress
  {
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNo { get; set; }
    public string Country { get; set; }
    public string State { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string PostalCode { get; set; }
  }

  public class ShippingAddress : BillingAddress
  {
    public bool SameToShippingAddress { get; set; }
  }

  public class ShippingMethod
  {
    public bool HomeDelivery { get; set; }
  }

}