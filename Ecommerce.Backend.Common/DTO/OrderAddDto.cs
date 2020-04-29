namespace Ecommerce.Backend.Common.DTO
{
  public class OrderAddDto
  {
    public string CustomerId { get; set; }
    public string ShippingMethodId { get; set; }
    public string PaymentMethodId { get; set; }
    public string Currency { get; set; }
  }
}
