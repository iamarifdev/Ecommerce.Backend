using System;
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
    [BsonElement("weight")]
    public double Weight { get; set; }

    [BsonElement("width")]
    public double Width { get; set; }

    [BsonElement("height")]
    public double Height { get; set; }

    [BsonElement("depth")]
    public double Depth { get; set; }
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

    [BsonElement("shippingDetail")]
    [BsonRequired]
    public ShippingDetail ShippingDetail { get; set; }

    [BsonElement("pricing")]
    [BsonRequired]
    public Pricing Pricing { get; set; }

    [BsonRequired]
    [BsonElement("imageUrl")]
    public string ImageUrl { get; set; }

    [BsonElement("isEnabled")]
    [BsonRequired]
    public bool IsEnabled { get; set; } = true;

    [BsonElement("isDeleted")]
    [BsonRequired]
    public bool IsDeleted { get; set; } = false;

  }
}