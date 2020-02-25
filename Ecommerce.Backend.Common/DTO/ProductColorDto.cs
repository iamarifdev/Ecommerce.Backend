using System.Collections.Generic;

namespace Ecommerce.Backend.Common.DTO
{
  public class ProductColorDto
  {
    public ProductColorDto() { }
    public ProductColorDto(string colorCode, string colorName)
    {
      ColorCode = colorCode;
      ColorName = colorName;
    }
    public string ColorCode { get; set; }
    public string ColorName { get; set; }
    public double? InStock { get; set; }
    public bool? IsAvailable { get; set; }
    public List<string> Images { get; set; }
    public List<double> Sizes { get; set; }
  }
}