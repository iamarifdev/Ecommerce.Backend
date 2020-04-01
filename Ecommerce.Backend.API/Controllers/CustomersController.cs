using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Ecommerce.Backend.Common.Helpers;
using Ecommerce.Backend.Common.DTO;
using Ecommerce.Backend.Common.Models;
using Ecommerce.Backend.Entities;
using Ecommerce.Backend.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Ecommerce.Backend.API.Controllers
{
  [SwaggerTag("Customers")]
  [Produces("application/json")]
  [Route("api/customers")]
  [ApiController]
  public class CustomersController : ControllerBase
  {
    private readonly IMapper _mapper;
    private readonly ICustomerService _customerService;
    private readonly ICustomer2FAVerificationService _verificationService;
    public CustomersController(ICustomerService customerService, ICustomer2FAVerificationService verificationService, IMapper mapper)
    {
      _mapper = mapper;
      _customerService = customerService;
      _verificationService = verificationService;
    }

    /// <summary>
    /// Get Pagniated customers
    /// </summary>
    [HttpGet("list")]
    public async Task<ActionResult<ApiResponse<PagedList<CustomerListItemDto>>>> GatPagedList([FromQuery] PagedQuery query)
    {
      try
      {
        var pagedCustomerList = await _customerService.GetPaginatedCustomerList(query);
        return pagedCustomerList.CreateSuccessResponse();
      }
      catch (Exception exception)
      {
        return BadRequest(exception.CreateErrorResponse());
      }
    }

    /// <summary>
    /// Get customer by Id
    /// </summary>
    /// <param name="customerId"></param>
    [HttpGet("{customerId}")]
    public async Task<ActionResult<ApiResponse<Customer>>> Get(String customerId)
    {
      try
      {
        var customer = await _customerService.GetById(customerId);
        return customer.CreateSuccessResponse();
      }
      catch (Exception exception)
      {
        return BadRequest(exception.CreateErrorResponse());
      }
    }

    /// <summary>
    /// Validate identity of a customer
    /// </summary>
    [HttpPost("validate/identity")]
    public async Task<ActionResult<ApiResponse<IdentityDto>>> ValidateIdentity(Dictionary<string, string> keyValues)
    {
      try
      {
        var isValid = await _customerService.ValidateIdentity(keyValues);
        var identiy = new IdentityDto { IsValid = isValid };
        return identiy.CreateSuccessResponse("Customer validated successfully!");
      }
      catch (Exception exception)
      {
        return BadRequest(exception.CreateErrorResponse());
      }
    }

    /// <summary>
    /// Add a new customer
    /// </summary>
    [HttpPost("add")]
    public async Task<ActionResult<ApiResponse<Customer>>> Add(CustomerAddDto dto)
    {
      try
      {
        var isVerified = await _verificationService.VerifyCode(dto.PhoneNo, dto.VerificationCode);
        if (!isVerified)
        {
          throw new Exception("Invalid verification code!");
        }
        var customer = _mapper.Map<Customer>(dto);
        var createdCustomer = await _customerService.Add(customer);
        return createdCustomer.CreateSuccessResponse("Customer created successfully!");
      }
      catch (Exception exception)
      {
        return BadRequest(exception.CreateErrorResponse());
      }
    }

    /// <summary>
    /// Update a customer by Id
    /// </summary>
    /// <param name="customerId"></param>
    [HttpPut("update/{customerId}")]
    public async Task<ActionResult<ApiResponse<Customer>>> Update(string customerId, Customer customer)
    {
      try
      {
        var updatedCustomer = await _customerService.UpdateById(customerId, customer);
        return updatedCustomer.CreateSuccessResponse("Customer updated successfully!");
      }
      catch (Exception exception)
      {
        return BadRequest(exception.CreateErrorResponse());
      }
    }

    /// <summary>
    /// Update customer billing address by Id
    /// </summary>
    /// <param name="customerId"></param>
    [HttpPatch("update/{customerId}/address/billing")]
    public async Task<ActionResult<ApiResponse<Customer>>> UpdateBillingAddress(string customerId, CustomerBillingAddressDto dto)
    {
      try
      {
        var billingAddress = _mapper.Map<BillingAddress>(dto);
        var updatedCustomer = await _customerService.UpdateBillingAddress(customerId, billingAddress);
        return updatedCustomer.CreateSuccessResponse("Customer billing address updated successfully!");
      }
      catch (Exception exception)
      {
        return BadRequest(exception.CreateErrorResponse());
      }
    }

    /// <summary>
    /// Update customer billing address by Id
    /// </summary>
    /// <param name="customerId"></param>
    [HttpPatch("update/{customerId}/address/shipping")]
    public async Task<ActionResult<ApiResponse<Customer>>> UpdateShippingAddress(string customerId, CustomerShippingAddressDto dto)
    {
      try
      {
        var shippingAddress = _mapper.Map<ShippingAddress>(dto);
        var updatedCustomer = await _customerService.UpdateShippingAddress(customerId, shippingAddress);
        return updatedCustomer.CreateSuccessResponse("Customer shipping address updated successfully!");
      }
      catch (Exception exception)
      {
        return BadRequest(exception.CreateErrorResponse());
      }
    }

    /// <summary>
    /// Activate a customer by Id
    /// </summary>
    /// <param name="customerId"></param>
    [HttpPut("activate/{customerId}")]
    public async Task<ActionResult<ApiResponse<Customer>>> ToggleActivation(string customerId, ActivateDto activateDto)
    {
      try
      {
        var updatedCustomer = await _customerService.ToggleActivationById(customerId, activateDto.Status);
        var status = activateDto.Status ? "activated" : "deactivated";
        return updatedCustomer.CreateSuccessResponse($"Customer {status} successfully!");
      }
      catch (Exception exception)
      {
        return BadRequest(exception.CreateErrorResponse());
      }
    }

    /// <summary>
    /// Delete a customer by Id
    /// </summary>
    /// <param name="customerId"></param>
    [HttpDelete("delete/{customerId}")]
    public async Task<ActionResult<ApiResponse<Customer>>> Delete(string customerId)
    {
      try
      {
        var deletedCustomer = await _customerService.RemoveById(customerId);
        return deletedCustomer.CreateSuccessResponse("Customer deleted successfully!");
      }
      catch (Exception exception)
      {
        return BadRequest(exception.CreateErrorResponse());
      }
    }
  }
}