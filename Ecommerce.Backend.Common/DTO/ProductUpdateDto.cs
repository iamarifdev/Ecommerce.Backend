using System.Collections.Generic;

namespace Ecommerce.Backend.Common.DTO
{
  public class ProductUpdateDto
  {
    public string ID { get; set; }
    public string SKU { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public ManufactureDetailDto ManufactureDetail { get; set; }
    public List<ProductColorDto> ProductColors { get; set; }
    public PricingDto Pricing { get; set; }
    public string FeatureImageUrl { get; set; }
    public bool IsEnabled { get; set; } = true;
  }
}