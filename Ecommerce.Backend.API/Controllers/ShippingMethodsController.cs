using System;
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
  }
}
