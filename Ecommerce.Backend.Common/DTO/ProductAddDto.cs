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

  public class ShippingDetailDto
  {
    public double? Weight { get; set; }
    public double? Width { get; set; }
    public double? Height { get; set; }
    public double? Depth { get; set; }
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
    public ShippingDetailDto ShippingDetail { get; set; }
    public PricingDto Pricing { get; set; }
    public List<string> Colors { get; set; }
  }
}