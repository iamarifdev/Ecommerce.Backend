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

  public class ShippingDetail
  {
    [BsonIgnoreIfNull]
    [BsonElement("weight")]
    public double? Weight { get; set; }

    [BsonIgnoreIfNull]
    [BsonElement("width")]
    public double? Width { get; set; }

    [BsonIgnoreIfNull]
    [BsonElement("height")]
    public double? Height { get; set; }

    [BsonIgnoreIfNull]
    [BsonElement("depth")]
    public double? Depth { get; set; }

    [BsonIgnoreIfNull]
    [BsonElement("sizes")]
    public List<double> Sizes { get; set; }
  }

  public class Product : BaseEntity
  {
    [BsonIgnoreIfNull]
    [BsonElement("sku")]
    public string SKU { get; set; }

    [BsonElement("inStock")]
    [BsonRequired]
    public double InStock { get; set; }

    [BsonElement("availibility")]
    [BsonRequired]
    public bool Availibility { get; set; } = true;

    [BsonElement("title")]
    [BsonRequired]
    public string Title { get; set; }

    [BsonElement("description")]
    [BsonRequired]
    public string Description { get; set; }

    [BsonElement("manufactureDetail")]
    [BsonRequired]
    public ManufactureDetail ManufactureDetail { get; set; }

    [BsonElement("shippingDetail")]
    [BsonRequired]
    public ShippingDetail ShippingDetail { get; set; }

    [BsonElement("pricing")]
    [BsonRequired]
    public Pricing Pricing { get; set; }

    [BsonElement("colors")]
    [BsonRequired]
    public List<string> Colors { get; set; }

    [BsonRequired]
    [BsonElement("featureImageUrl")]
    public string FeatureImageUrl { get; set; }

    [BsonElement("images")]
    [BsonRequired]
    public List<string> Images { get; set; }

    [BsonElement("isEnabled")]
    [BsonRequired]
    public bool IsEnabled { get; set; } = true;

    [BsonElement("isDeleted")]
    [BsonRequired]
    public bool IsDeleted { get; set; } = false;

  }
}