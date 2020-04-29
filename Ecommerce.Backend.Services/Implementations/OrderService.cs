using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Backend.Common.DTO;
using Ecommerce.Backend.Common.Helpers;
using Ecommerce.Backend.Common.Models;
using Ecommerce.Backend.Entities;
using Ecommerce.Backend.Services.Abstractions;

namespace Ecommerce.Backend.Services.Implementations
{
  public class OrderService : BaseService<Order>, IOrderService
  {
    private readonly ICartService _cartService;

    public OrderService(ICartService cartService)
    {
      _cartService = cartService;
    }
    private OrderProduct _prepareOrderProduct(CartProduct product)
    {
      var orderProduct = new OrderProduct
      {
        Color = product.Color,
        ProductRef = product.ProductId.ToReference<Product>(),
        Quantity = product.Quantity,
        Size = product.Size,
        TotalPrice = product.UnitPrice * (decimal) product.Quantity,
        UnitPrice = product.UnitPrice
      };
      return orderProduct;
    }

    private Order _prepareOrderFromCart(Cart cart, CustomerTransactionSession session = null)
    {
      var newOrder = new Order
      {
      CustomerRef = session.IsEmpty() ? cart.CustomerId.ToReference<Customer>() : session.CustomerRef,
      OrderProducts = cart.Products.Select(cartProduct => _prepareOrderProduct(cartProduct)),
      OrderStatus = session.IsEmpty() ? OrderStatus.OrderReceived : OrderStatus.PaymentReceived
      };
      return newOrder;
    }

    public async Task<PagedList<OrderListItemDto>> GetPaginatedOrderList(PagedQuery query)
    {
      var items = await GetPaginatedList<OrderListItemDto>(query, order => new OrderListItemDto(order));
      return items;
    }

    public async Task<Order> AddOrderWithoutPayment(OrderAddDto dto)
    {
      var cart = await _cartService.GetCartById(customerId: dto.CustomerId);
      var newOrder = _prepareOrderFromCart(cart);
      newOrder.PaymentMethodRef = dto.PaymentMethodId.ToReference<PaymentMethod>();
      newOrder.ShippingMethodRef = dto.ShippingMethodId.ToReference<ShippingMethod>();
      newOrder.Trackings.Prepend(new OrderTracking
      {
        Description = OrderTracking.GetDescription(newOrder.OrderStatus),
        Status = newOrder.OrderStatus
      });

      await Add(newOrder);
      await _cartService.ToggleActivationById(cart.ID, false);
      return newOrder;
    }

  }
}