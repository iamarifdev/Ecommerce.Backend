using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities;

namespace Ecommerce.Backend.Entities
{
  public class UserPhoneNumber
  {
    [BsonElement("phoneNo")]
    [BsonRequired]
    public string PhoneNo { get; set; }
  }
  public class UserAddress
  {
    [BsonElement("district")]
    [BsonRequired]
    public string District { get; set; }

    [BsonElement("thana")]
    [BsonRequired]
    public string Thana { get; set; }

    [BsonElement("postCode")]
    [BsonRequired]
    public string PostCode { get; set; }

    [BsonElement("description")]
    [BsonRequired]
    public string Description { get; set; }
  }
  public class User : BaseEntityWithStatus
  {
    public User() : base(new BaseEntityWithStatus { IsEnabled = false }) { }

    [BsonElement("roleRef")]
    [BsonRequired]
    public One<Role> RoleRef { get; set; }

    [BsonElement("username")]
    [BsonRequired]
    public string Username { get; set; }

    [JsonIgnore]
    [BsonElement("password")]
    [BsonIgnoreIfNull]
    public string Password { get; set; }

    [JsonIgnore]
    [BsonElement("salt")]
    [BsonIgnoreIfNull]
    public string Salt { get; set; }

    [JsonIgnore]
    [BsonIgnoreIfNull]
    [BsonElement("passwordResetToken")]
    public string PasswordResetToken { get; set; }

    [JsonIgnore]
    [BsonIgnoreIfNull]
    [BsonElement("passwordResetExpires")]
    public DateTime? PasswordResetExpires { get; set; }

    [BsonIgnoreIfNull]
    [BsonElement("fullName")]
    [BsonRequired]
    public string FullName { get; set; }

    [BsonElement("email")]
    [BsonRequired]
    public string Email { get; set; }

    [BsonElement("phoneNumbers")]
    [BsonRequired]
    public IEnumerable<UserPhoneNumber> PhoneNumbers { get; set; }

    [BsonIgnoreIfNull]
    [BsonElement("contactPerson")]
    public string ContactPerson { get; set; }

    [BsonIgnoreIfNull]
    [BsonElement("contactNo")]
    public string ContactNo { get; set; }

    [BsonElement("addresses")]
    [BsonRequired]
    public IEnumerable<UserAddress> Addresses { get; set; }

    [BsonElement("remarks")]
    [BsonIgnoreIfNull]
    public string Remarks { get; set; }

    [BsonIgnoreIfNull]
    [BsonElement("avatarUrl")]
    public string AvatarUrl { get; set; }

    [BsonIgnoreIfNull]
    [BsonElement("createdByRef")]
    public One<User> CreatedByRef { get; set; }

    [BsonIgnoreIfNull]
    [BsonElement("updatedByRef")]
    public One<User> UpdatedByRef { get; set; }
  }
}