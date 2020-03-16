using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities;

namespace Ecommerce.Backend.Entities
{
  public enum OrderStatus
  {
    PaymentReceived = 1,
    Shipped = 2,
    HandOverToCourier = 3,
    DeliveredToCustomer = 4,
    ReceivedByCustomer = 5
  }

  public class Order : BaseEntityWithStatus
  {
    [BsonElement("customer")]
    [BsonRequired]
    public One<Customer> Customer { get; set; }

    [BsonElement("orderProducts")]
    [BsonRequired]
    public IEnumerable<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();

    [BsonElement("shippingMethod")]
    [BsonRequired]
    public One<ShippingMethod> ShippingMethod { get; set; }

    [BsonElement("paymentMethod")]
    [BsonRequired]
    public One<PaymentMethod> PaymentMethod { get; set; }

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
    public One<Product> Product { get; set; }

    [BsonElement("quantity")]
    [BsonRequired]
    public double Quantity { get; set; }

    [BsonElement("unitPrice")]
    [BsonRequired]
    public double UnitPrice { get; set; }

    [BsonElement("color")]
    [BsonRequired]
    public string Color { get; set; }

    [BsonElement("size")]
    [BsonRequired]
    public double Size { get; set; }

    [BsonIgnoreIfNull]
    [BsonElement("totalPrice")]
    public double TotalPrice { get; set; }
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
    public DateTime UpdatedAt { get; set; }
  }
}
