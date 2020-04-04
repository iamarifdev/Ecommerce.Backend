using System;
using System.Threading.Tasks;
using AutoMapper;
using Ecommerce.Backend.API.Helpers;
using Ecommerce.Backend.Common.DTO;
using Ecommerce.Backend.Common.Models;
using Ecommerce.Backend.Entities;
using Ecommerce.Backend.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Ecommerce.Backend.API.Admin.Controllers
{
  [SwaggerTag("Admin Orders")]
  [Produces("application/json")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [Authorize]
  [Route("admin/api/orders")]
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
    public async Task<ActionResult<ApiResponse<Order>>> Add(Order order)
    {
      try
      {
        // var order = _mapper.Map<Order>(dto);
        var createdOrder = await _orderService.Add(order);
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

    /// <summary>
    /// Activate a order by Id
    /// </summary>
    /// <param name="orderId"></param>
    [HttpPut("activate/{orderId}")]
    public async Task<ActionResult<ApiResponse<Order>>> ToggleActivation(string orderId, ActivateDto activateDto)
    {
      try
      {
        var updatedOrder = await _orderService.ToggleActivationById(orderId, activateDto.Status);
        var status = activateDto.Status ? "activated" : "deactivated";
        return updatedOrder.CreateSuccessResponse($"Order {status} successfully!");
      }
      catch (Exception exception)
      {
        return BadRequest(exception.CreateErrorResponse());
      }
    }

    /// <summary>
    /// Delete a order by Id
    /// </summary>
    /// <param name="orderId"></param>
    [HttpDelete("delete/{orderId}")]
    public async Task<ActionResult<ApiResponse<Order>>> Delete(string orderId)
    {
      try
      {
        var deletedOrder = await _orderService.RemoveById(orderId);
        return deletedOrder.CreateSuccessResponse("Order deleted successfully!");
      }
      catch (Exception exception)
      {
        return BadRequest(exception.CreateErrorResponse());
      }
    }
  }
}