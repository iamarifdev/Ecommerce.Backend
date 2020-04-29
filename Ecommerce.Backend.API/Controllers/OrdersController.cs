using System;
using System.Threading.Tasks;
using AutoMapper;
using Ecommerce.Backend.API.Helpers;
using Ecommerce.Backend.Common.DTO;
using Ecommerce.Backend.Common.Models;
using Ecommerce.Backend.Entities;
using Ecommerce.Backend.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Ecommerce.Backend.API.Controllers
{
  [SwaggerTag("Orders")]
  [Produces("application/json")]
  [Route("api/orders")]
  [ApiController]
  public class OrdersController : ControllerBase
  {
    private readonly IMapper _mapper;
    private readonly IOrderService _orderService;
    public OrdersController(IOrderService orderService, IMapper mapper)
    {
      _mapper = mapper;
      _orderService = orderService;
    }

    /// <summary>
    /// Get Pagniated orders
    /// </summary>
    [HttpGet("list")]
    public async Task<ActionResult<ApiResponse<PagedList<OrderListItemDto>>>> GatPagedOrderList([FromQuery] PagedQuery query)
    {
      try
      {
        // TODO: filter with customer ID to get only specific customer orders
        var pagedOrderList = await _orderService.GetPaginatedOrderList(query);
        return pagedOrderList.CreateSuccessResponse();
      }
      catch (Exception exception)
      {
        return BadRequest(exception.CreateErrorResponse());
      }
    }

    /// <summary>
    /// Get order by Id
    /// </summary>
    /// <param name="orderId"></param>
    [HttpGet("{orderId}")]
    public async Task<ActionResult<ApiResponse<Order>>> Get(String orderId)
    {
      try
      {
        var order = await _orderService.GetById(orderId);
        return order.CreateSuccessResponse();
      }
      catch (Exception exception)
      {
        return BadRequest(exception.CreateErrorResponse());
      }
    }

    /// <summary>
    /// Add a new order
    /// </summary>
    [HttpPost("add")]
    public async Task<ActionResult<ApiResponse<Order>>> Add(OrderAddDto dto)
    {
      try
      {
        var createdOrder = await _orderService.AddOrderWithoutPayment(dto);
        return createdOrder.CreateSuccessResponse("Order created successfully!");
      }
      catch (Exception exception)
      {
        return BadRequest(exception.CreateErrorResponse());
      }
    }

    /// <summary>
    /// Update a order by Id
    /// </summary>
    /// <param name="orderId"></param>
    [HttpPut("update/{orderId}")]
    public async Task<ActionResult<ApiResponse<Order>>> Update(string orderId, Order order)
    {
      try
      {
        var updatedOrder = await _orderService.UpdateById(orderId, order);
        return updatedOrder.CreateSuccessResponse("Order updated successfully!");
      }
      catch (Exception exception)
      {
        return BadRequest(exception.CreateErrorResponse());
      }
    }
  }
}