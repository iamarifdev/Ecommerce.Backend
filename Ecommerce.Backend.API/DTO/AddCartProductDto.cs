using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Backend.API.DTO
{
  public class AddCartProductDto
  {
    public string CustomerId { get; set; }

    [Required]
    public string ProductId { get; set; }

    [Required]
    public double Quantity { get; set; }

    [Required]
    public string Color { get; set; }

    [Required]
    public double Size { get; set; }
  }
}