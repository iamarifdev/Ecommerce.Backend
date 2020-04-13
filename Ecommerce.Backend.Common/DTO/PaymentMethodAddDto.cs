namespace Ecommerce.Backend.Common.DTO
{
  public class PaymentMethodAddDto
  {
    public string MethodName { get; set; }
    public bool HasPaymentGateway { get; set; }
    public string IconName { get; set; }
    public bool IsEnabled { get; set; }
  }
}