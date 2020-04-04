using MongoDB.Bson.Serialization.Attributes;

namespace Ecommerce.Backend.Entities
{
  public class BaseEntityWithStatus : BaseEntity
  {
    public BaseEntityWithStatus()
    {
      IsDeleted = false;
      IsEnabled = true;
    }

    public BaseEntityWithStatus(BaseEntityWithStatus status)
    {
      IsDeleted = status.IsDeleted;
      IsEnabled = status.IsEnabled;
    }

    [BsonElement("isEnabled")]
    [BsonRequired]
    public bool IsEnabled { get; set; }

    [BsonElement("isDeleted")]
    [BsonRequired]
    public bool IsDeleted { get; set; }
  }
}