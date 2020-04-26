using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Ecommerce.Backend.API.Helpers;
using Ecommerce.Backend.Common.Models;
using Ecommerce.Backend.Services.Abstractions;
using Ecommerce.PaymentGateway.SSLCommerz.Configurations;
using Ecommerce.PaymentGateway.SSLCommerz.Models;
using Ecommerce.PaymentGateway.SSLCommerz.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Swashbuckle.AspNetCore.Annotations;

namespace Ecommerce.Backend.API.Controllers
{
  [SwaggerTag("Payments")]
  [Route("api/payments")]
  public class PaymentsController : Controller
  {
    private readonly IMapper _mapper;
    private readonly ISSLCommerzService _sslCommerzService;
    private readonly ICustomerTransactionSessionService _customerTransactionSessionService;
    private readonly ICartService _cartService;
    private readonly ISSLCommerzConfig _config;
    private readonly ICustomerTransactionService _customerTransactionService;

    public PaymentsController(
      IMapper mapper,
      ISSLCommerzConfig config,
      ICartService cartService,
      ISSLCommerzService sslCommerzService,
      ICustomerTransactionService customerTransactionService,
      ICustomerTransactionSessionService customerTransactionSessionService
    )
    {
      _mapper = mapper;
      _config = config;
      _cartService = cartService;
      _sslCommerzService = sslCommerzService;
      _customerTransactionService = customerTransactionService;
      _customerTransactionSessionService = customerTransactionSessionService;
    }

    /// <summary>
    /// Initiate SSLCommerze transaction
    /// </summary>
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpPost("transaction/initiate")]
    public async Task<ActionResult<ApiResponse<InitResponse>>> InitiateTransaction([FromBody] Dictionary<string, string> parameters)
    {
      try
      {
        if (!parameters.ContainsKey("value_a"))
        {
          throw new Exception("Customer ID is not found");
        }
        if (!parameters.ContainsKey("currency"))
        {
          throw new Exception("Currency is not found");
        }

        var transactionId = ObjectId.GenerateNewId().ToString();
        parameters.Add("tran_id", transactionId);

        var customerId = parameters["value_a"];
        var currency = parameters["currency"];

        parameters = await _cartService.GetCartDetailToOrder(customerId, parameters);
        var initResponse = await _sslCommerzService.InitiateTransaction(parameters);
        var session = await _customerTransactionSessionService.AddSession(customerId, transactionId, currency, initResponse.SessionKey);
        if (session == null)
        {
          throw new Exception("Invalid Session");
        }
        return initResponse.CreateSuccessResponse("Payment transaction initiated successfully!");
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
    public async Task<ActionResult> CompleteTransaction(IFormCollection keyValues, IPN ipn)
    {
      try
      {
        if (!keyValues.ContainsKey("value_a"))
        {
          throw new Exception("Customer ID is not found");
        }

        var customerId = keyValues["value_a"].ToString();
        var session = await _customerTransactionSessionService.GetSessionByCustomerId(customerId);
        if (session == null)
        {
          throw new Exception("Invalid session");
        }

        var(isValidated, message) = await _sslCommerzService.ValidateTransaction(ipn.TransactionId, session.Amount, session.Currency, keyValues);
        if (isValidated)
        {
          var isTransactionExist = await _customerTransactionService.IsTransactionExist(session.SessionKey, session.TransactionId);
          if (isTransactionExist)
          {
            await _customerTransactionSessionService.RemoveById(session.ID);
            return Ok(new { isValidated, message = "Transaction already exist." });
          }
          await _customerTransactionService.AddTransaction(session, ipn);
          await _customerTransactionSessionService.RemoveById(session.ID);
        }
        // TODO: save order information
        if (isValidated)
        {
          Response.Headers.Add("amount", ipn.Amount.ToString());
          return Redirect(_config.APP.GetSuccessUrl());
        }
        return Ok(new { isValidated, message });
      }
      catch (Exception exception)
      {
        return BadRequest(exception.CreateErrorResponse());
      }
    }
  }
}