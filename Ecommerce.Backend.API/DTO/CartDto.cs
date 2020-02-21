using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Backend.API.DTO
{
  public class CartProductDto
  {
    [Required]
    public string ProductId { get; set; }

    [Required]
    public string SKU { get; set; }

    [Required]
    public string Title { get; set; }

    [Required]
    public int Quantity { get; set; }

    [Required]
    public double UnitPrice { get; set; }
    public double TotalPrice { get; set; }
  }

  public class CartDto
  {
    public string CustomerId { get; set; }
    public string Status { get; set; }
    public int Quantity { get; set; }
    public double TotalPrice { get; set; }

    [Required]
    public List<CartProductDto> Products { get; set; } = new List<CartProductDto>();
  }
}