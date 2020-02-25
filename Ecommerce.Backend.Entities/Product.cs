using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace Ecommerce.Backend.Entities
{
  public class ManufactureDetail
  {
    [BsonElement("modelNo")]
    [BsonRequired]
    public string ModelNo { get; set; }

    [BsonElement("releaseDate")]
    [BsonRequired]
    public DateTime ReleaseDate { get; set; }
  }

  public class Pricing
  {
    [BsonElement("price")]
    [BsonRequired]
    public double Price { get; set; }
  }

  public class ProductColor
  {
    [BsonElement("colorCode")]
    [BsonRequired]
    public string ColorCode { get; set; }

    [BsonElement("colorName")]
    [BsonRequired]
    public string ColorName { get; set; }

    [BsonElement("inStock")]
    [BsonRequired]
    public double InStock { get; set; }

    [BsonElement("isAvailable")]
    [BsonRequired]
    public bool IsAvailable { get; set; } = true;

    [BsonElement("images")]
    [BsonIgnoreIfNull]
    public List<string> Images { get; set; }

    [BsonElement("sizes")]
    [BsonRequired]
    public List<double> Sizes { get; set; }
  }

  public class Product : BaseEntity
  {
    [BsonIgnoreIfNull]
    [BsonElement("sku")]
    public string SKU { get; set; }

    [BsonElement("title")]
    [BsonRequired]
    public string Title { get; set; }

    [BsonElement("description")]
    [BsonRequired]
    public string Description { get; set; }

    [BsonElement("manufactureDetail")]
    [BsonRequired]
    public ManufactureDetail ManufactureDetail { get; set; }

    [BsonElement("productColors")]
    [BsonRequired]
    public List<ProductColor> ProductColors { get; set; }

    [BsonElement("pricing")]
    [BsonRequired]
    public Pricing Pricing { get; set; }

    [BsonRequired]
    [BsonElement("featureImageUrl")]
    public string FeatureImageUrl { get; set; }

    [BsonElement("isEnabled")]
    [BsonRequired]
    public bool IsEnabled { get; set; } = true;

    [BsonElement("isDeleted")]
    [BsonRequired]
    public bool IsDeleted { get; set; } = false;
  }
}