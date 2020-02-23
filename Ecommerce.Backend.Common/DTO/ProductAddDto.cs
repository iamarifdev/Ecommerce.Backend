using System;
using System.Collections.Generic;

namespace Ecommerce.Backend.Common.DTO
{
  public class ManufactureDetailDto
  {
    public string ModelNo { get; set; }
    public DateTime ReleaseDate { get; set; }
  }

  public class PricingDto
  {
    public double Price { get; set; }
  }

  public class ProductColorDto
  {
    public string Color { get; set; }
    public double InStock { get; set; }
    public bool IsAvailable { get; set; } = true;
    public List<string> Images { get; set; }
    public PricingDto Pricing { get; set; }
    public List<double> Sizes { get; set; }
  }
  public class ProductAddDto
  {
    public string SKU { get; set; }
    public double InStock { get; set; }
    public bool Availibility { get; set; } = true;
    public string Title { get; set; }
    public string Description { get; set; }
    public ManufactureDetailDto ManufactureDetail { get; set; }
    public List<ProductColorDto> ProductColors { get; set; }
    public bool IsEnabled { get; set; } = true;
  }
}