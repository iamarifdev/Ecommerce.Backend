using MongoDB.Bson.Serialization.Attributes;

namespace Ecommerce.Backend.Entities
{
  public class ShippingMethod : BaseEntityWithStatus
  {
    [BsonElement("methodName")]
    [BsonRequired]
    public string MethodName { get; set; }

    [BsonElement("cost")]
    [BsonRequired]
    public double Cost { get; set; } = 0;

    [BsonElement("isOutSide")]
    [BsonRequired]
    public bool IsOutSide {get; set; } = false;
  }
}
