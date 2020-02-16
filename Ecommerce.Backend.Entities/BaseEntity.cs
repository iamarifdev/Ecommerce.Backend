using System;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities.Core;

namespace Ecommerce.Backend.Entities
{
  public class BaseEntity : Entity
  {
    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [BsonElement("updatedAt")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
  }
}