using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Ecommerce.Backend.Entities
{
  public class Customer2FAVerification : BaseEntityWithStatus
  {
    [BsonElement("phoneNo")]
    [BsonRequired]
    public string PhoneNo { get; set; }

    [BsonElement("verficationCode")]
    [BsonRequired]
    public string VerficationCode { get; set; }

    [BsonElement("isVerified")]
    [BsonRequired]
    public bool IsVerified { get; set; } = false;

    [BsonElement("expiresAt")]
    [BsonRequired]
    public DateTime ExpiresAt { get; set; }
  }
}