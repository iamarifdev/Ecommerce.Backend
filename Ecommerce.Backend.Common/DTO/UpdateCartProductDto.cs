namespace Ecommerce.Backend.Common.DTO
{
  public class UpdateCartProductDto
  {
    public string CartProductId { get; set; }
    public double Quantity { get; set; }
    public string ProductId { get; set; }
  }
}