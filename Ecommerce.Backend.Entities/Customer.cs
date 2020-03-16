using MongoDB.Bson.Serialization.Attributes;

namespace Ecommerce.Backend.Entities
{
  public class Customer : BaseEntityWithStatus
  {
    [BsonElement("phoneNo")]
    [BsonIgnoreIfNull]
    public string PhoneNo { get; set; }

    [BsonElement("email")]
    [BsonIgnoreIfNull]
    public string Email { get; set; }

    [BsonElement("firstName")]
    [BsonIgnoreIfNull]
    public string FirstName { get; set; }

    [BsonElement("lastName")]
    [BsonIgnoreIfNull]
    public string LastName { get; set; }

    [BsonElement("billingAddress")]
    [BsonIgnoreIfNull]
    public BillingAddress BillingAddress { get; set; }

    [BsonElement("shippingAddress")]
    [BsonIgnoreIfNull]
    public ShippingAddress ShippingAddress { get; set; }

    [BsonElement("avatarUrl")]
    [BsonIgnoreIfNull]
    public string AvatarUrl { get; set; }

    /// <summary>
    /// Indicates profile completeness in % (percentage)
    /// </summary>
    [BsonElement("profileCompleteness")]
    [BsonIgnoreIfNull]
    public int ProfileCompleteness { get; set; }
  }

  public class BillingAddress
  {
    [BsonElement("phoneNo")]
    [BsonRequired]
    public string PhoneNo { get; set; }

    [BsonElement("email")]
    [BsonIgnoreIfNull]
    public string Email { get; set; }

    [BsonElement("firstName")]
    [BsonRequired]
    public string FirstName { get; set; }

    [BsonElement("lastName")]
    [BsonRequired]
    public string LastName { get; set; }

    [BsonElement("country")]
    [BsonRequired]
    public string Country { get; set; }

    [BsonElement("state")]
    [BsonRequired]
    public string State { get; set; }

    [BsonElement("address")]
    [BsonRequired]
    public string Address { get; set; }

    [BsonElement("city")]
    [BsonRequired]
    public string City { get; set; }

    [BsonElement("postalCode")]
    [BsonRequired]
    public string PostalCode { get; set; }
  }

  public class ShippingAddress : BillingAddress
  {
    [BsonElement("sameToShippingAddress")]
    [BsonRequired]
    public bool SameToShippingAddress { get; set; }
  }
}
