using MongoDB.Bson.Serialization.Attributes;

namespace Ecommerce.Backend.Entities
{
  public class BaseEntityWithStatus : BaseEntity
  {
    [BsonElement("isEnabled")]
    [BsonRequired]
    public bool IsEnabled { get; set; } = true;

    [BsonElement("isDeleted")]
    [BsonRequired]
    public bool IsDeleted { get; set; } = false;
  }
}
