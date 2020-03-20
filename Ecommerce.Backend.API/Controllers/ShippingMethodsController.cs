using System;
using System.Threading.Tasks;
using AutoMapper;
using Ecommerce.Backend.API.Helpers;
using Ecommerce.Backend.Common.DTO;
using Ecommerce.Backend.Common.Models;
using Ecommerce.Backend.Entities;
using Ecommerce.Backend.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Backend.API.Controllers
{
  [Route("api/shipping-methods")]
  [ApiController]

  public class ShippingMethodsController : ControllerBase
  {
    private readonly IMapper _mapper;
    private readonly IShippingMethodService _shippingMethodService;
    public ShippingMethodsController(IShippingMethodService shippingMethodService, IMapper mapper)
    {
      _mapper = mapper;
      _shippingMethodService = shippingMethodService;
    }

    /// <summary>
    /// Get Pagniated shipping methods
    /// </summary>
    [HttpGet("list")]
    public async Task<ActionResult<ApiResponse<PagedList<ShippingMethod>>>> GatPagedShippingMethodList([FromQuery] PagedQuery query)
    {
      try
      {
        var pagedShippingMethodList = await _shippingMethodService.GetPaginatedList(query);
        return pagedShippingMethodList.CreateSuccessResponse();
      }
      catch (Exception exception)
      {
        return BadRequest(exception.CreateErrorResponse());
      }
    }

    /// <summary>
    /// Get shipping method by Id
    /// </summary>
    /// <param name="shippingMethodId"></param>
    [HttpGet("{shippingMethodId}")]
    public async Task<ActionResult<ApiResponse<ShippingMethod>>> Get(String shippingMethodId)
    {
      try
      {
        var shippingMethod = await _shippingMethodService.GetById(shippingMethodId);
        return shippingMethod.CreateSuccessResponse();
      }
      catch (Exception exception)
      {
        return BadRequest(exception.CreateErrorResponse());
      }
    }

    /// <summary>
    /// Add a new shipping method
    /// </summary>
    [HttpPost("add")]
    public async Task<ActionResult<ApiResponse<ShippingMethod>>> Add(ShippingMethodAddDto dto)
    {
      try
      {
        var shippingMethod = _mapper.Map<ShippingMethod>(dto);
        var createdShippingMethod = await _shippingMethodService.Add(shippingMethod);
        return createdShippingMethod.CreateSuccessResponse("Shipping method created successfully!");
      }
      catch (Exception exception)
      {
        return BadRequest(exception.CreateErrorResponse());
      }
    }

    /// <summary>
    /// Update a shipping method by Id
    /// </summary>
    /// <param name="shippingMethodId"></param>
    [HttpPut("update/{shippingMethodId}")]
    public async Task<ActionResult<ApiResponse<ShippingMethod>>> Update(string shippingMethodId, ShippingMethod shippingMethod)
    {
      try
      {
        var updatedShippingMethod = await _shippingMethodService.UpdateById(shippingMethodId, shippingMethod);
        return updatedShippingMethod.CreateSuccessResponse("Shipping method updated successfully!");
      }
      catch (Exception exception)
      {
        return BadRequest(exception.CreateErrorResponse());
      }
    }

    /// <summary>
    /// Activate a shipping method by Id
    /// </summary>
    /// <param name="shippingMethodId"></param>
    [HttpPut("activate/{shippingMethodId}")]
    public async Task<ActionResult<ApiResponse<ShippingMethod>>> ToggleActivation(string shippingMethodId, ActivateDto activateDto)
    {
      try
      {
        var updatedShippingMethod = await _shippingMethodService.ToggleActivationById(shippingMethodId, activateDto.Status);
        var status = activateDto.Status ? "activated" : "deactivated";
        return updatedShippingMethod.CreateSuccessResponse($"Shipping method {status} successfully!");
      }
      catch (Exception exception)
      {
        return BadRequest(exception.CreateErrorResponse());
      }
    }

    /// <summary>
    /// Delete a shipping method by Id
    /// </summary>
    /// <param name="shippingMethodId"></param>
    [HttpDelete("delete/{shippingMethodId}")]
    public async Task<ActionResult<ApiResponse<ShippingMethod>>> Delete(string shippingMethodId)
    {
      try
      {
        var deletedShippingMethod = await _shippingMethodService.RemoveById(shippingMethodId);
        return deletedShippingMethod.CreateSuccessResponse("Shipping method deleted successfully!");
      }
      catch (Exception exception)
      {
        return BadRequest(exception.CreateErrorResponse());
      }
    }
  }
}
