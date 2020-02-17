using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace Ecommerce.Backend.Entities
{
  // [BsonId]
  public class CartProduct
  {
    [BsonIgnoreIfNull]
    [BsonElement("sku")]
    public string SKU { get; set; }

    [BsonElement("title")]
    [BsonRequired]
    public string Title { get; set; }

    [BsonElement("quantity")]
    [BsonRequired]
    public int Quantity { get; set; }

    [BsonElement("price")]
    [BsonRequired]
    public double Price { get; set; }
  }

  public class Cart : BaseEntity
  {
    [BsonIgnoreIfNull]
    [BsonElement("status")]
    public string Status { get; set; }

    [BsonElement("quantity")]
    [BsonRequired]
    public int Quantity { get; set; }

    [BsonElement("total")]
    [BsonRequired]
    public double Total { get; set; }

    [BsonElement("products")]
    [BsonRequired]
    public List<CartProduct> Products { get; set; } = new List<CartProduct>();
  }
}