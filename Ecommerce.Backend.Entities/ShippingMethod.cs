namespace Ecommerce.Backend.Entities
{
  public class ShippingMethod : BaseEntity
  {
    public string MethodName { get; set; }
    public double Cost { get; set; } = 0;
    public bool IsOutSide {get; set; } = false;
    public bool IsEnabled { get; set; } = true;
    public bool IsDeleted { get; set; } = false;
  }
}
