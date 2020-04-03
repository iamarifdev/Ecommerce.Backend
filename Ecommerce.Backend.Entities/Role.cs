using MongoDB.Bson.Serialization.Attributes;

namespace Ecommerce.Backend.Entities
{
  public class Role : BaseEntityWithStatus
  {
    [BsonRequired]
    [BsonElement("name")]
    public string Name { get; set; }

    [BsonRequired]
    [BsonElement("description")]
    public string Description { get; set; }
  }
}