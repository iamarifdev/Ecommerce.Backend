using System;
using System.Threading.Tasks;
using Ecommerce.Backend.API.Helpers;
using Ecommerce.Backend.Common.DTO;
using Ecommerce.Backend.Common.Models;
using Ecommerce.Backend.Entities;
using Ecommerce.Backend.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Ecommerce.Backend.API.Controllers
{
  [SwaggerTag("Admin Auth")]
  [Produces("application/json")]
  [Route("admin/api/auth")]
  [ApiController]
  public class AuthController : ControllerBase
  {
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
      _authService = authService;
    }

    /// <summary>
    /// Seed Database
    /// </summary>
    [HttpPost("/api/seed/database")]
    public async Task<ActionResult<ApiResponse<User>>> SeedDatabase()
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
    /// Login User
    /// </summary>
    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<AuthUserDto>>> Login(LoginDto dto)
    {
      try
      {
        var authUser = await _authService.Authenticate(dto.Username, dto.Password);
        if (authUser == null) throw new UnauthorizedAccessException("Unauthorized access!");
        return authUser.CreateSuccessResponse("User logged in successfully");
      }
      catch (Exception exception)
      {
        return Unauthorized(exception.CreateErrorResponse());
      }
    }

    /// <summary>
    /// Refresh token of a user
    /// </summary>
    [HttpPost("token/refresh")]
    public async Task<ActionResult<ApiResponse<RefreshTokenDto>>> RefreshToken(RefreshTokenDto dto)
    {
      try
      {
        var refreshToken = await _authService.RefreshAccessToken(dto);
        return refreshToken.CreateSuccessResponse("Refresh token successfully generated.");
      }
      catch (Exception exception)
      {
        return Unauthorized(exception.CreateErrorResponse());
      }
    }

    /// <summary>
    /// Logout User
    /// </summary>
    [HttpPost("logout")]
    public async Task<ActionResult<ApiResponse<bool>>> Logout(LogoutDto dto)
    {
      try
      {
        var isLoggedOut = await _authService.LogOut(dto.Username, dto.RefreshToken);
        return isLoggedOut.CreateSuccessResponse("User logged out.");
      }
      catch (Exception exception)
      {
        return Unauthorized(exception.CreateErrorResponse());
      }
    }

    /// <summary>
    /// Logout from all device
    /// </summary>
    [HttpPost("logout/all")]
    public async Task<ActionResult<ApiResponse<bool>>> LogoutAll(LogoutDto dto)
    {
      try
      {
        var(allLoggedOut, deviceCount) = await _authService.LogOutFromAllDevice(dto.Username, dto.RefreshToken);
        return allLoggedOut.CreateSuccessResponse($"User logged out from {deviceCount} devices.");
      }
      catch (Exception exception)
      {
        return Unauthorized(exception.CreateErrorResponse());
      }
    }
  }
}