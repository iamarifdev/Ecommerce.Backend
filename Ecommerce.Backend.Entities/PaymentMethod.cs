namespace Ecommerce.Backend.Entities
{
  public class PaymentMethod : BaseEntity
  {
    public string MethodName { get; set; }
    public bool HasPaymentGateway { get; set; }
    public bool IsEnabled { get; set; } = true;
    public bool IsDeleted { get; set; } = false;
  }
}
