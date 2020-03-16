using System;
using System.Collections.Generic;
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
    public One<ShippingMethod> ShippingMethod { get; set; }
    public One<PaymentMethod> PaymentMethod { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public IEnumerable<OrderTracking> Trackings { get; set; } = new List<OrderTracking>();
  }

  public class OrderTracking
  {
    public OrderStatus Status { get; set; }
    public string Description { get; set; }
    public DateTime UpdatedAt { get; set; }
  }
}
