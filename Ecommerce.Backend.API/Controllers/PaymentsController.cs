using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Ecommerce.Backend.API.Helpers;
using Ecommerce.Backend.Common.Models;
using Ecommerce.PaymentGateway.SSLCommerz.Models;
using Ecommerce.PaymentGateway.SSLCommerz.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Ecommerce.Backend.API.Controllers
{
  [SwaggerTag("Payments")]
  [Route("api/payments")]
  public class PaymentsController : Controller
  {
    private readonly IMapper _mapper;
    private readonly ISSLCommerzService _sslCommerzService;
    public PaymentsController(ISSLCommerzService sslCommerzService, IMapper mapper)
    {
      _mapper = mapper;
      _sslCommerzService = sslCommerzService;
    }

    /// <summary>
    /// Initiate SSLCommerze transaction
    /// </summary>
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpPost("transaction/initiate")]
    public async Task<ActionResult<ApiResponse<InitResponse>>> InitiateTransaction(Dictionary<string, string> parameters)
    {
      try
      {
        var response = await _sslCommerzService.InitiateTransaction(parameters);
        return response.CreateSuccessResponse("Payment transaction initiated successfully!");
      }
      catch (Exception exception)
      {
        return BadRequest(exception.CreateErrorResponse());
      }
    }

    /// <summary>
    /// Handle Successfull transaction
    /// </summary>
    [HttpPost("transaction/success")]
    public ActionResult CompleteTransaction(IFormCollection keyValues, IPN ipn)
    {
      try
      {
        var isVerified = _sslCommerzService.VerifyIPNHash(keyValues);
        return Ok(new { ipn, isVerified });
      }
      catch (Exception exception)
      {
        return BadRequest(exception.CreateErrorResponse());
      }
    }
  }
}