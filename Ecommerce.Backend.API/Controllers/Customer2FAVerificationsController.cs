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
  [Route("api/customer/verifications")]
  [ApiController]

  public class Customer2FAVerificationsController : ControllerBase
  {
    private readonly IMapper _mapper;
    private readonly ICustomer2FAVerificationService _verificationService;
    public Customer2FAVerificationsController(ICustomer2FAVerificationService verificationService, IMapper mapper)
    {
      _mapper = mapper;
      _verificationService = verificationService;
    }

    /// <summary>
    /// Get Pagniated customer 2FA verifications
    /// </summary>
    [HttpGet("list")]
    public async Task<ActionResult<ApiResponse<PagedList<Customer2FAVerification>>>> GatPagedCustomer2FAVerificationList([FromQuery] PagedQuery query)
    {
      try
      {
        var pagedCustomer2FAVerificationList = await _verificationService.GetPaginatedList(query);
        return pagedCustomer2FAVerificationList.CreateSuccessResponse();
      }
      catch (Exception exception)
      {
        return BadRequest(exception.CreateErrorResponse());
      }
    }

    /// <summary>
    /// Get customer 2FA verification by Id
    /// </summary>
    /// <param name="verificationId"></param>
    [HttpGet("{verificationId}")]
    public async Task<ActionResult<ApiResponse<Customer2FAVerification>>> Get(String verificationId)
    {
      try
      {
        var customer2FAverification = await _verificationService.GetById(verificationId);
        return customer2FAverification.CreateSuccessResponse();
      }
      catch (Exception exception)
      {
        return BadRequest(exception.CreateErrorResponse());
      }
    }

    /// <summary>
    /// Add a new customer 2FA verification
    /// </summary>
    [HttpPost("add")]
    public async Task<ActionResult<ApiResponse<string>>> Add(Customer2FAVerificationAddDto dto)
    {
      try
      {
        await _verificationService.InvalidateAllVerificationCode(dto.PhoneNo);
        var verificationCode = _verificationService.GenerateVerificationCode();
        var createdCustomer2FAVerification = await _verificationService.Add(
          new Customer2FAVerification
          {
            PhoneNo = dto.PhoneNo,
            VerficationCode = verificationCode,
            ExpiresAt = DateTime.Now.AddMinutes(5)
          }
        );
        return createdCustomer2FAVerification.PhoneNo.CreateSuccessResponse("Verfication code created successfully!");
      }
      catch (Exception exception)
      {
        return BadRequest(exception.CreateErrorResponse());
      }
    }

    /// <summary>
    /// Verify a customer 2FA verification code
    /// </summary>
    /// <param name="phoneNo"></param>
    [HttpPatch("verify/{phoneNo}")]
    public async Task<ActionResult<ApiResponse<bool>>> Verify(string phoneNo, Customer2FAVerificationUpdateDto dto)
    {
      try
      {
        if (dto.PhoneNo != phoneNo)
        {
          throw new Exception("Invalid phone number!");
        }
        var isExist = await _verificationService.IsVerificationCodeExist(dto.PhoneNo, dto.VerificationCode);
        if (!isExist)
        {
          throw new Exception("Invalid verification code!");
        }
        var verified = await _verificationService.VerifyCode(dto.PhoneNo, dto.VerificationCode);
        return verified.CreateSuccessResponse("Phone number verified with verification code successfully!");
      }
      catch (Exception exception)
      {
        return BadRequest(exception.CreateErrorResponse());
      }
    }

    /// <summary>
    /// Delete a customer 2FA verification by Id
    /// </summary>
    /// <param name="verificationId"></param>
    [HttpDelete("delete/{verificationId}")]
    public async Task<ActionResult<ApiResponse<Customer2FAVerification>>> Delete(string verificationId)
    {
      try
      {
        var deletedCustomer2FAVerification = await _verificationService.RemoveById(verificationId);
        return deletedCustomer2FAVerification.CreateSuccessResponse("Verfication code deleted successfully!");
      }
      catch (Exception exception)
      {
        return BadRequest(exception.CreateErrorResponse());
      }
    }
  }
}