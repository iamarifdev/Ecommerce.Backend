using System;
using System.Collections.Generic;
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
  [SwaggerTag("Payment Methods")]
  [Produces("application/json")]
  [Route("api/payment-methods")]
  [ApiController]
  public class PaymentMethodsController : ControllerBase
  {
    private readonly IMapper _mapper;
    private readonly IPaymentMethodService _paymentMethodService;
    public PaymentMethodsController(IPaymentMethodService paymentMethodService, IMapper mapper)
    {
      _mapper = mapper;
      _paymentMethodService = paymentMethodService;
    }

    /// <summary>
    /// Get all payment methods
    /// </summary>
    [HttpGet("list")]
    public async Task<ActionResult<ApiResponse<List<PaymentMethod>>>> GatPaymentMethodList([FromQuery] Query query)
    {
      try
      {
        var pagedPaymentMethodList = await _paymentMethodService.GetList(query);
        return pagedPaymentMethodList.CreateSuccessResponse();
      }
      catch (Exception exception)
      {
        return BadRequest(exception.CreateErrorResponse());
      }
    }

    /// <summary>
    /// Get payment method by Id
    /// </summary>
    /// <param name="paymentMethodId"></param>
    [HttpGet("{paymentMethodId}")]
    public async Task<ActionResult<ApiResponse<PaymentMethod>>> Get(string paymentMethodId)
    {
      try
      {
        var paymentMethod = await _paymentMethodService.GetById(paymentMethodId);
        return paymentMethod.CreateSuccessResponse();
      }
      catch (Exception exception)
      {
        return BadRequest(exception.CreateErrorResponse());
      }
    }
  }
}
