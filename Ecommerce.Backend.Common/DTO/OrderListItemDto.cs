using System;
using System.Collections.Generic;
using System.Linq;
using Ecommerce.Backend.Entities;

namespace Ecommerce.Backend.Common.DTO
{
  public class OrderListItemDto
  {
    public OrderListItemDto() { }
    public OrderListItemDto(Order order)
    {
      _initialize(order);
    }
    public string ID { get; set; }
    public OrderCustomerDto Customer { get; set; }
    public OrderPaymentMethodDto PaymentMethod { get; set; }
    public OrderShippingMethodDto ShippingMethod { get; set; }
    public IEnumerable<OrderProductDto> OrderProducts { get; set; } = new List<OrderProductDto>();
    public string OrderStatus { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    private void _initialize(Order order)
    {
      var customer = order.CustomerRef.ToEntity();
      var paymentMethod = order.PaymentMethodRef.ToEntity();
      var shippingMethod = order.ShippingMethodRef.ToEntity();
      ID = order.ID;
      Customer = new OrderCustomerDto
      {
        AvatarUrl = customer.AvatarUrl,
        Email = customer.Email,
        PhoneNo = customer.PhoneNo,
        FirstName = customer.FirstName,
        LastName = customer.LastName
      };
      PaymentMethod = new OrderPaymentMethodDto
      {
        ID = paymentMethod.ID,
        HasPaymentGateway = paymentMethod.HasPaymentGateway,
        MethodName = paymentMethod.MethodName
      };
      ShippingMethod = new OrderShippingMethodDto
      {
        ID = shippingMethod.ID,
        Cost = shippingMethod.Cost,
        IsOutSide = shippingMethod.IsOutSide,
        MethodName = shippingMethod.MethodName
      };
      OrderProducts = order.OrderProducts.Select(s =>
      {
        var product = s.ProductRef.ToEntity();
        var orderProduct = new OrderProductDto
        {
          ID = s.ID,
          Color = s.Color,
          ProductTitle = product.Title,
          Quantity = s.Quantity,
          Size = s.Size,
          SKU = product.SKU,
          TotalPrice = s.TotalPrice,
          UnitPrice = s.UnitPrice
        };
        return orderProduct;
      });
      OrderStatus = order.OrderStatus.ToString();
      CreatedAt = order.CreatedAt;
      UpdatedAt = order.UpdatedAt;
    }
  }

  public class OrderCustomerDto
  {
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNo { get; set; }
    public string Email { get; set; }
    public string AvatarUrl { get; set; }
    public CustomerBillingAddressDto BillingAddress { get; set; }
    public CustomerShippingAddressDto ShippingAddress { get; set; }
  }

  public class OrderPaymentMethodDto
  {
    public string ID { get; set; }
    public string MethodName { get; set; }
    public bool HasPaymentGateway { get; set; }
  }

  public class OrderShippingMethodDto
  {
    public string ID { get; set; }
    public string MethodName { get; set; }
    public double Cost { get; set; }
    public bool IsOutSide { get; set; }
  }

  public class OrderProductDto
  {
    public string ID { get; set; }
    public string ProductTitle { get; set; }
    public string SKU { get; set; }
    public string Color { get; set; }
    public double? Size { get; set; }
    public double? Quantity { get; set; }
    public double? TotalPrice { get; set; }
    public double? UnitPrice { get; set; }
  }
}