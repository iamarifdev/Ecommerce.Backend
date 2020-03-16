using MongoDB.Bson.Serialization.Attributes;

namespace Ecommerce.Backend.Entities
{
  public class PaymentMethod : BaseEntityWithStatus
  {
    [BsonElement("methodName")]
    [BsonRequired]
    public string MethodName { get; set; }

    [BsonElement("hasPaymentGateway")]
    [BsonRequired]
    public bool HasPaymentGateway { get; set; }
  }
}
