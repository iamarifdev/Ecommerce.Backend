using MongoDB.Bson.Serialization.Attributes;

namespace Ecommerce.Backend.Entities
{
  public class UserLogin: BaseEntityWithStatus
  {
    [BsonElement("userLoginId")]
    [BsonRequired]
    public int UserLoginId { get; set; }

    [BsonElement("userId")]
    [BsonRequired]
    public string UserId { get; set; }

    [BsonElement("accessToken")]
    [BsonRequired]
    public string AccessToken { get; set; }

    [BsonElement("refreshToken")]
    [BsonRequired]
    public string RefreshToken { get; set; }
  }
}