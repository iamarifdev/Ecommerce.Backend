using System.Collections.Generic;

namespace Ecommerce.Backend.Common.DTO
{
  public class CartProductDto
  {
    public string ProductId { get; set; }

    public string SKU { get; set; }

    public string Title { get; set; }

    public int Quantity { get; set; }

    public double UnitPrice { get; set; }
    public double TotalPrice { get; set; }
  }

  public class CartDto
  {
    public string CustomerId { get; set; }
    public string Status { get; set; }
    public int Quantity { get; set; }
    public double TotalPrice { get; set; }
    public List<CartProductDto> Products { get; set; } = new List<CartProductDto>();
  }
}