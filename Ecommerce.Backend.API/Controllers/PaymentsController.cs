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
  [Produces("application/json")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [Route("api/payments")]
  [ApiController]
  public class PaymentsController : ControllerBase
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
    [HttpPost("transactions/initiate")]
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
  }
}