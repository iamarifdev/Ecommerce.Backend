namespace Ecommerce.Backend.Common.DTO
{
  public class ShippingMethodAddDto
  {
    public string MethodName { get; set; }
    public double Cost { get; set; }
    public string IconName { get; set; }
    public bool IsOutSide { get; set; }
    public bool IsEnabled { get; set; }
  }
}
