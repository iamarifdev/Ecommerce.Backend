using System.Collections.Generic;

namespace Ecommerce.Backend.Common.DTO
{
  public class ProductListItemDto
  {
    public string ID { get; set; }
    public string SKU { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public ManufactureDetailDto ManufactureDetail { get; set; }
    public PricingDto Pricing { get; set; }
    public IEnumerable<ProductColorDto> ProductColors { get; set; }
  }
}