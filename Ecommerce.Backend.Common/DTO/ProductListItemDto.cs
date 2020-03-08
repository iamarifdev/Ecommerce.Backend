using System;
using System.Collections.Generic;

namespace Ecommerce.Backend.Common.DTO
{
  public class ProductListItemDto
  {
    public ProductListItemDto() { }
    public ProductListItemDto(
      string id,
      string sku,
      string title,
      string description,
      string featureImageUrl,
      ManufactureDetailDto manufactureDetail,
      PricingDto pricing,
      IEnumerable<ProductColorDto> productColors,
      bool isEnabled,
      DateTime createdAt,
      DateTime updatedAt
    )
    {
      ID = id;
      SKU = sku;
      Title = title;
      Description = description;
      FeatureImageUrl = featureImageUrl;
      ManufactureDetail = manufactureDetail;
      Pricing = pricing;
      ProductColors = productColors;
      IsEnabled = isEnabled;
      CreatedAt = createdAt;
      UpdatedAt = updatedAt;
    }
    public string ID { get; set; }
    public string SKU { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string FeatureImageUrl { get; set; }
    public ManufactureDetailDto ManufactureDetail { get; set; }
    public PricingDto Pricing { get; set; }
    public IEnumerable<ProductColorDto> ProductColors { get; set; }
    public bool IsEnabled { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
  }
}