namespace Ecommerce.Backend.Common.DTO
{
  public class AddCartProductDto
  {
    public string CartId { get; set; }
    public string CustomerId { get; set; }
    public string ProductId { get; set; }
    public double Quantity { get; set; }
    public string Color { get; set; }
    public double Size { get; set; }
  }
}