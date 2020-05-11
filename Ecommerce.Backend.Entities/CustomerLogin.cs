using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Ecommerce.Backend.Entities
{
  public class CustomerLogin : BaseEntityWithStatus
  {
    [BsonElement("customerLoginId")]
    [BsonRequired]
    public int CustomerLoginId { get; set; }

    [BsonElement("customerId")]
    [BsonRequired]
    public string CustomerId { get; set; }

    [BsonElement("accessToken")]
    [BsonRequired]
    public string AccessToken { get; set; }

    [BsonElement("refreshToken")]
    [BsonRequired]
    public string RefreshToken { get; set; }
  }
}