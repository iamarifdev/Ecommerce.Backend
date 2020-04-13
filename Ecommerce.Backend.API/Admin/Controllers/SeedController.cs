using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ecommerce.Backend.API.Helpers;
using Ecommerce.Backend.Common.Models;
using Ecommerce.Backend.Entities;
using Ecommerce.Backend.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Ecommerce.Backend.API.Admin.Controllers
{
  [SwaggerTag("Seed")]
  [Produces("application/json")]
  [Route("admin/api/seed")]
  [ApiController]
  public class SeedController : ControllerBase
  {
    private readonly IAuthService _authService;
    private readonly IShippingMethodService _shippingMethodService;
    private readonly IPaymentMethodService _paymentMethodService;

    public SeedController(IAuthService authService, IShippingMethodService shippingMethodService, IPaymentMethodService paymentMethodService)
    {
      _authService = authService;
      _paymentMethodService = paymentMethodService;
      _shippingMethodService = shippingMethodService;
    }

    /// <summary>
    /// Seed Database
    /// </summary>
    [HttpPost("user")]
    public async Task<ActionResult<ApiResponse<User>>> SeedUser()
    {
      try
      {
        var(role, user) = await _authService.SeedDatabase();
        if (role == null || user == null)
        {
          throw new InvalidOperationException();
        }
        return user.CreateSuccessResponse("Database seeded succesfully");
      }
      catch (Exception exception)
      {
        return BadRequest(exception.CreateErrorResponse());
      }
    }

    /// <summary>
    /// Seed Configuration
    /// </summary>
    [HttpPost("configuration")]
    public async Task<ActionResult> SeedConfiguration()
    {
      try
      {
        if (!await _paymentMethodService.IsExist(p => p.IsEnabled))
        {
          var paymentMethods = new List<PaymentMethod>();
          paymentMethods.Add(new PaymentMethod
          {
            MethodName = "Online Payment",
              IconName = "credit card.svg",
              HasPaymentGateway = true,
              IsEnabled = true
          });
          paymentMethods.Add(new PaymentMethod
          {
            MethodName = "Cash on Delivery",
              IconName = "cash-on-delivery.svg",
              HasPaymentGateway = false,
              IsEnabled = true
          });
          await _paymentMethodService.AddRange(paymentMethods);
        }

        if (!await _shippingMethodService.IsExist(p => p.IsEnabled))
        {
          var paymentMethods = new List<ShippingMethod>();
          paymentMethods.Add(new ShippingMethod
          {
            MethodName = "Courier",
              IconName = "delivered.svg",
              Cost = 100,
              IsOutSide = true,
              IsEnabled = true
          });
          paymentMethods.Add(new ShippingMethod
          {
            MethodName = "Home Delivery",
              IconName = "delivery.svg",
              Cost = 30,
              IsOutSide = false,
              IsEnabled = true
          });
          await _shippingMethodService.AddRange(paymentMethods);
        }
        return Ok();
      }
      catch (Exception exception)
      {
        return BadRequest(exception.CreateErrorResponse());
      }
    }
  }
}