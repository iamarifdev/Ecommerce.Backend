using System.Collections.Generic;

namespace Ecommerce.Backend.API.DTO
{
  public class CartProductDto
  {
    public string SKU { get; set; }
    public string Title { get; set; }
    public int Quantity { get; set; }
    public double Price { get; set; }
  }

  public class CartDto
  {
    public string CustomerId { get; set; }
    public string Status { get; set; }
    public int Quantity { get; set; }
    public double Total { get; set; }
    public List<CartProductDto> Products { get; set; } = new List<CartProductDto>();
  }
}