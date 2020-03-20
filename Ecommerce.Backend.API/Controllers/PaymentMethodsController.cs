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
    /// Get Pagniated payment methods
    /// </summary>
    [HttpGet("list")]
    public async Task<ActionResult<ApiResponse<PagedList<PaymentMethod>>>> GatPagedPaymentMethodList([FromQuery] PagedQuery query)
    {
      try
      {
        var pagedPaymentMethodList = await _paymentMethodService.GetPaginatedList(query);
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
    public async Task<ActionResult<ApiResponse<PaymentMethod>>> Get(String paymentMethodId)
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

    /// <summary>
    /// Add a new payment method
    /// </summary>
    [HttpPost("add")]
    public async Task<ActionResult<ApiResponse<PaymentMethod>>> Add(PaymentMethodAddDto dto)
    {
      try
      {
        var paymentMethod = _mapper.Map<PaymentMethod>(dto);
        var createdPaymentMethod = await _paymentMethodService.Add(paymentMethod);
        return createdPaymentMethod.CreateSuccessResponse("Payment method created successfully!");
      }
      catch (Exception exception)
      {
        return BadRequest(exception.CreateErrorResponse());
      }
    }

    /// <summary>
    /// Update a payment method by Id
    /// </summary>
    /// <param name="paymentMethodId"></param>
    [HttpPut("update/{paymentMethodId}")]
    public async Task<ActionResult<ApiResponse<PaymentMethod>>> Update(string paymentMethodId, PaymentMethod paymentMethod)
    {
      try
      {
        var updatedPaymentMethod = await _paymentMethodService.UpdateById(paymentMethodId, paymentMethod);
        return updatedPaymentMethod.CreateSuccessResponse("Payment method updated successfully!");
      }
      catch (Exception exception)
      {
        return BadRequest(exception.CreateErrorResponse());
      }
    }

    /// <summary>
    /// Activate a payment method by Id
    /// </summary>
    /// <param name="paymentMethodId"></param>
    [HttpPut("activate/{paymentMethodId}")]
    public async Task<ActionResult<ApiResponse<PaymentMethod>>> ToggleActivation(string paymentMethodId, ActivateDto activateDto)
    {
      try
      {
        var updatedPaymentMethod = await _paymentMethodService.ToggleActivationById(paymentMethodId, activateDto.Status);
        var status = activateDto.Status ? "activated" : "deactivated";
        return updatedPaymentMethod.CreateSuccessResponse($"Payment method {status} successfully!");
      }
      catch (Exception exception)
      {
        return BadRequest(exception.CreateErrorResponse());
      }
    }

    /// <summary>
    /// Delete a payment method by Id
    /// </summary>
    /// <param name="paymentMethodId"></param>
    [HttpDelete("delete/{paymentMethodId}")]
    public async Task<ActionResult<ApiResponse<PaymentMethod>>> Delete(string paymentMethodId)
    {
      try
      {
        var deletedPaymentMethod = await _paymentMethodService.RemoveById(paymentMethodId);
        return deletedPaymentMethod.CreateSuccessResponse("Payment method deleted successfully!");
      }
      catch (Exception exception)
      {
        return BadRequest(exception.CreateErrorResponse());
      }
    }
  }
}
