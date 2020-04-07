using System;
using AutoMapper;
using Ecommerce.Backend.API.Helpers;
using Ecommerce.PaymentGateway.SSLCommerz.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Ecommerce.Backend.API.Admin.Controllers
{
  [SwaggerTag("Admin Payments")]
  [Produces("application/json")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [Route("admin/api/payments")]
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
    /// Recieve IPN from PaymentGateway Service
    /// </summary>
    [HttpPost("listen-ipn")]
    public ActionResult ListenIPN(HttpRequest request)
    {
      try
      {
        Console.Write("IPN Received>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> body:"+ request.Body.ToString());
        Console.Write("IPN Received>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> form:"+ request.Form.ToString());
        Console.Write("IPN Received>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> query:"+ request.Query.ToString());
        return Ok(request.Body.ToString());
      }
      catch (Exception exception)
      {
        return BadRequest(exception.CreateErrorResponse());
      }
    }
  }
}