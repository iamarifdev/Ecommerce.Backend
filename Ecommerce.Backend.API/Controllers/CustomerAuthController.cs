using System;
using System.Threading.Tasks;
using Ecommerce.Backend.API.Helpers;
using Ecommerce.Backend.Common.DTO;
using Ecommerce.Backend.Common.Models;
using Ecommerce.Backend.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Ecommerce.Backend.API.Controllers
{
  [SwaggerTag("Customer Auth")]
  [Produces("application/json")]
  [Route("api/auth")]
  [ApiController]
  public class CustomerAuthController : ControllerBase
  {
    private readonly ICustomerAuthService _customerAuthService;

    public CustomerAuthController(ICustomerAuthService customerAuthService)
    {
      _customerAuthService = customerAuthService;
    }

    /// <summary>
    /// Authenticate customer
    /// </summary>
    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<AuthUserDto>>> Login(CustomerLoginDto dto)
    {
      try
      {
        var authUser = await _customerAuthService.Authenticate(dto.PhoneNo, dto.Password);
        if (authUser == null) throw new Exception("Invalid phone number or password.");
        return authUser.CreateSuccessResponse("Logged in successfully");
      }
      catch (Exception exception)
      {
        return BadRequest(exception.CreateErrorResponse());
      }
    }

    /// <summary>
    /// Refresh token of a customer
    /// </summary>
    [HttpPost("token/refresh")]
    public async Task<ActionResult<ApiResponse<RefreshTokenDto>>> RefreshToken(RefreshTokenDto dto)
    {
      try
      {
        var refreshToken = await _customerAuthService.RefreshAccessToken(dto);
        return refreshToken.CreateSuccessResponse("Refresh token successfully generated.");
      }
      catch (Exception exception)
      {
        return Unauthorized(exception.CreateErrorResponse());
      }
    }

    /// <summary>
    /// Logout Customer
    /// </summary>
    [HttpPost("logout")]
    public async Task<ActionResult<ApiResponse<bool>>> Logout(CustomerLogoutDto dto)
    {
      try
      {
        var isLoggedOut = await _customerAuthService.LogOut(dto.PhoneNo, dto.RefreshToken);
        return isLoggedOut.CreateSuccessResponse("Customer logged out");
      }
      catch (Exception exception)
      {
        return Unauthorized(exception.CreateErrorResponse());
      }
    }

    /// <summary>
    /// Logout Customer from all device
    /// </summary>
    [HttpPost("logout/all")]
    public async Task<ActionResult<ApiResponse<bool>>> LogoutAll(CustomerLogoutDto dto)
    {
      try
      {
        var(allLoggedOut, deviceCount) = await _customerAuthService.LogOutFromAllDevice(dto.PhoneNo, dto.RefreshToken);
        return allLoggedOut.CreateSuccessResponse($"Customer logged out from {deviceCount} devices.");
      }
      catch (Exception exception)
      {
        return Unauthorized(exception.CreateErrorResponse());
      }
    }
  }
}