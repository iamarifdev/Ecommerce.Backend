using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities;

namespace Ecommerce.Backend.Entities
{
  public class CustomerTransaction : BaseEntityWithStatus
  {
    [BsonRequired]
    [BsonElement("sessionKey")]
    public string SessionKey { get; set; }

    [BsonRequired]
    [BsonElement("customerRef")]
    public One<Customer> CustomerRef { get; set; }

    [BsonIgnoreIfNull]
    [BsonElement("orderRef")]
    public One<Order> OrderRef { get; set; }

    [BsonRequired]
    [BsonElement("amount")]
    public decimal Amount { get; set; }

    [BsonRequired]
    [BsonElement("currency")]
    public string Currency { get; set; }

    [BsonRequired]
    [BsonElement("bankTransactionId")]
    public string BankTransactionId { get; set; }

    [BsonRequired]
    [BsonElement("cardNo")]
    public string CardNo { get; set; }

    [BsonRequired]
    [BsonElement("cardType")]
    public string CardType { get; set; }

    [BsonRequired]
    [BsonElement("status")]
    public string Status { get; set; }

    [BsonRequired]
    [BsonElement("storeAmount")]
    public decimal StoreAmount { get; set; }

    [BsonRequired]
    [BsonElement("storeId")]
    public string StoreId { get; set; }

    [BsonRequired]
    [BsonElement("transactionId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string TransactionId { get; set; }

    [BsonRequired]
    [BsonElement("transactionDate")]
    public DateTime TransactionDate { get; set; }
  }

  public class CustomerTransactionSession : BaseEntityWithStatus
  {
    [BsonRequired]
    [BsonElement("sessionKey")]
    public string SessionKey { get; set; }

    [BsonRequired]
    [BsonElement("transactionId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string TransactionId { get; set; }

    [BsonRequired]
    [BsonElement("customerRef")]
    public One<Customer> CustomerRef { get; set; }

    [BsonRequired]
    [BsonElement("cartRef")]
    public One<Cart> CartRef { get; set; }

    [BsonRequired]
    [BsonElement("amount")]
    public decimal Amount { get; set; }

    [BsonRequired]
    [BsonElement("currency")]
    public string Currency { get; set; }
  }
}