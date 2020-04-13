using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Ecommerce.Backend.API.Helpers;
using Ecommerce.Backend.Common.Models;
using Ecommerce.Backend.Entities;
using Ecommerce.Backend.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Ecommerce.Backend.API.Controllers
{
  [SwaggerTag("Shipping Methods")]
  [Produces("application/json")]
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
    /// Get shipping method list
    /// </summary>
    [HttpGet("list")]
    public async Task<ActionResult<ApiResponse<List<ShippingMethod>>>> GatShippingMethodList([FromQuery] Query query)
    {
      try
      {
        var pagedShippingMethodList = await _shippingMethodService.GetList(query);
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
  }
}
