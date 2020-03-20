namespace Ecommerce.Backend.Common.DTO
{
  public class OrderListItemDto
  {
    // todo update
    public string ID { get; set; }
    public CustomerListItemDto Customer { get; set; }
    public string PaymentMethodName { get; set; }
    public string ShippingMethodName { get; set; }
  }
}