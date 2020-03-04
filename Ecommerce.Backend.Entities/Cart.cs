using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities;

namespace Ecommerce.Backend.Entities
{
  public static class CartStatus
  {
    public const string Active = "active";
    public const string InActive = "inactive";
  }

  public class CartProduct
  {
    public CartProduct()
    {
      TotalPrice = UnitPrice * Quantity;
    }

    [BsonElement("id")]
    public string ID { get; set; } = ObjectId.GenerateNewId().ToString();

    [BsonElement("productId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string ProductId { get; set; }

    [BsonIgnoreIfNull]
    [BsonElement("sku")]
    public string SKU { get; set; }

    [BsonElement("title")]
    [BsonRequired]
    public string Title { get; set; }

    [BsonElement("quantity")]
    [BsonRequired]
    public double Quantity { get; set; }

    [BsonElement("unitPrice")]
    [BsonRequired]
    public double UnitPrice { get; set; }

    [BsonElement("color")]
    [BsonRequired]
    public string Color { get; set; }

    [BsonElement("size")]
    [BsonRequired]
    public double Size { get; set; }

    [BsonIgnoreIfNull]
    [BsonElement("totalPrice")]
    public double TotalPrice { get; set; }

    [Ignore]
    public string FeatureImageUrl { get; set; }
  }

  public class Cart : BaseEntity
  {
    [BsonElement("customerId")]
    [BsonRequired]
    public string CustomerId { get; set; }

    [BsonIgnoreIfNull]
    [BsonElement("status")]
    public string Status { get; set; } = CartStatus.Active;

    [BsonElement("quantity")]
    [BsonRequired]
    public double Quantity { get; set; }

    [BsonElement("totalPrice")]
    [BsonRequired]
    public double TotalPrice { get; set; }

    [BsonElement("products")]
    [BsonRequired]
    public List<CartProduct> Products { get; set; } = new List<CartProduct>();
  }
}