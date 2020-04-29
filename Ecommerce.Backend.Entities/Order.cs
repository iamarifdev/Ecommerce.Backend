using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities;

namespace Ecommerce.Backend.Entities
{
  public enum OrderStatus
  {
    OrderReceived = 1,
    PaymentReceived = 2,
    Confirmed = 3,
    Shipped = 4,
    HandOverToCourier = 5,
    DeliveredToCustomer = 6,
    ReceivedByCustomer = 7,
    CancelledByCustomer = 8,
    Cancelled = 8,
  }

  public class Order : BaseEntityWithStatus
  {
    [BsonElement("customer")]
    [BsonRequired]
    public One<Customer> CustomerRef { get; set; }

    [BsonElement("transactionId")]
    [BsonIgnoreIfNull]
    [BsonRepresentation(BsonType.ObjectId)]
    public string TransactionId { get; set; }

    [BsonElement("orderProducts")]
    [BsonRequired]
    public IEnumerable<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();

    [BsonElement("shippingMethod")]
    [BsonRequired]
    public One<ShippingMethod> ShippingMethodRef { get; set; }

    [BsonElement("paymentMethod")]
    [BsonRequired]
    public One<PaymentMethod> PaymentMethodRef { get; set; }

    [BsonElement("orderStatus")]
    [BsonRequired]
    public OrderStatus OrderStatus { get; set; }

    [BsonElement("trackings")]
    [BsonRequired]
    public IEnumerable<OrderTracking> Trackings { get; set; } = new List<OrderTracking>();
  }

  public class OrderProduct
  {
    [BsonElement("id")]
    public string ID { get; set; } = ObjectId.GenerateNewId().ToString();

    [BsonElement("product")]
    [BsonRequired]
    public One<Product> ProductRef { get; set; }

    [BsonElement("quantity")]
    [BsonRequired]
    public double Quantity { get; set; }

    [BsonElement("unitPrice")]
    [BsonRequired]
    public decimal UnitPrice { get; set; }

    [BsonElement("color")]
    [BsonRequired]
    public string Color { get; set; }

    [BsonElement("size")]
    [BsonRequired]
    public double Size { get; set; }

    [BsonIgnoreIfNull]
    [BsonElement("totalPrice")]
    public decimal TotalPrice { get; set; }
  }

  public class OrderTracking
  {
    [BsonElement("status")]
    [BsonRequired]
    public OrderStatus Status { get; set; }

    [BsonElement("description")]
    [BsonRequired]
    public string Description { get; set; }

    [BsonElement("updatedAt")]
    [BsonRequired]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public static string GetDescription(OrderStatus status)
    {
      var map = new Dictionary<OrderStatus, string>();
      map.Add(OrderStatus.OrderReceived, "Order has been received, waiting to be confirmed");
      map.Add(OrderStatus.PaymentReceived, "Order with payment has been received, waiting to be confirmed");
      map.Add(OrderStatus.Confirmed, "Order has been confirmed, waiting to be shipped");
      map.Add(OrderStatus.Shipped, "Ordered items has been shipped");
      map.Add(OrderStatus.HandOverToCourier, "Ordered items has been handed over to courier");
      map.Add(OrderStatus.DeliveredToCustomer, "Ordered items has been delivered to customer");
      map.Add(OrderStatus.CancelledByCustomer, "Ordered has been cancelled by customer");
      map.Add(OrderStatus.Cancelled, "Ordered has been cancelled");
      return map[status];
    }
  }
}