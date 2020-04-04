using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Ecommerce.Backend.API.Helpers;
using Ecommerce.Backend.Common.DTO;
using Ecommerce.Backend.Common.Helpers;
using Ecommerce.Backend.Common.Models;
using Ecommerce.Backend.Entities;
using Ecommerce.Backend.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Ecommerce.Backend.API.Admin.Controllers
{
  [SwaggerTag("Admin Users")]
  [Produces("application/json")]
  [Authorize]
  [Route("admin/api/users")]
  [ApiController]
  public class UsersController : ControllerBase
  {
    private readonly HttpClient _http;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public UsersController(IMapper mapper, IUserService userService, EcommerceHttpClient httpClient)
    {
      _mapper = mapper;
      _http = httpClient.Http;
      _userService = userService;
    }

    /// <summary>
    /// Get Pagniated Users
    /// </summary>
    [HttpGet("list")]
    public async Task<ActionResult<ApiResponse<PagedList<User>>>> GatPagedUserList([FromQuery] PagedQuery query)
    {
      try
      {
        var pagedUserList = await _userService.GetPaginatedList(query);
        return pagedUserList.CreateSuccessResponse();
      }
      catch (Exception exception)
      {
        return BadRequest(exception.CreateErrorResponse());
      }
    }

    /// <summary>
    /// Get User by ID
    /// </summary>
    /// <param name="userId"></param>
    [HttpGet("{userId}")]
    public async Task<ActionResult<ApiResponse<User>>> Get(String userId)
    {
      try
      {
        var user = await _userService.GetById(userId);
        return user.CreateSuccessResponse();
      }
      catch (Exception exception)
      {
        return BadRequest(exception.CreateErrorResponse());
      }
    }

    /// <summary>
    /// Validate identity of a user
    /// </summary>
    [HttpPost("validate/identity")]
    public async Task<ActionResult<ApiResponse<IdentityDto>>> ValidateIdentity(Dictionary<string, string> keyValues)
    {
      try
      {
        var isValid = await _userService.ValidateIdentity(keyValues);
        var identiy = new IdentityDto { IsValid = isValid };
        return identiy.CreateSuccessResponse("Customer validated successfully!");
      }
      catch (Exception exception)
      {
        return BadRequest(exception.CreateErrorResponse());
      }
    }

    /// <summary>
    /// Add a new user
    /// </summary>
    [HttpPost("add")]
    public async Task<ActionResult<ApiResponse<User>>> Add(UserAddDto dto)
    {
      try
      {
        var user = _mapper.Map<User>(dto);
        var createdUser = await _userService.Add(_mapper.Map<User>(user));
        return createdUser.CreateSuccessResponse("User created successfully!");
      }
      catch (Exception exception)
      {
        return BadRequest(exception.CreateErrorResponse());
      }
    }

    /// <summary>
    /// Update a user by Id
    /// </summary>
    /// <param name="userId"></param>
    [HttpPut("update/{userId}")]
    public async Task<ActionResult<ApiResponse<User>>> Update(string userId, UserUpdateDto dto)
    {
      try
      {
        var user = _mapper.Map<User>(dto);
        var updatedUser = await _userService.UpdateById(userId, user);
        return updatedUser.CreateSuccessResponse("User updated successfully!");
      }
      catch (Exception exception)
      {
        return BadRequest(exception.CreateErrorResponse());
      }
    }

    /// <summary>
    /// Activate a user by Id
    /// </summary>
    /// <param name="userId"></param>
    [HttpPut("activate/{userId}")]
    public async Task<ActionResult<ApiResponse<User>>> ToggleActivation(string userId, ActivateDto activateDto)
    {
      try
      {
        var updatedUser = await _userService.ToggleActivationById(userId, activateDto.Status);
        var status = activateDto.Status ? "activated" : "deactivated";
        return updatedUser.CreateSuccessResponse($"User {status} successfully!");
      }
      catch (Exception exception)
      {
        return BadRequest(exception.CreateErrorResponse());
      }
    }

    /// <summary>
    /// Delete a user by Id
    /// </summary>
    /// <param name="userId"></param>
    [HttpDelete("delete/{userId}")]
    public async Task<ActionResult<ApiResponse<User>>> Delete(string userId)
    {
      try
      {
        var updatedUser = await _userService.RemoveById(userId);
        return updatedUser.CreateSuccessResponse("User deleted successfully!");
      }
      catch (Exception exception)
      {
        return BadRequest(exception.CreateErrorResponse());
      }
    }

    /// <summary>
    /// User avatar upload
    /// </summary>
    /// <param name="userId"></param>
    [HttpPatch("upload/avatar/{userId}")]
    public async Task<ActionResult<ApiResponse<string>>> UploadProfile(string userId, IFormFile avatar)
    {
      try
      {
        var formData = new MultipartFormDataContent();
        formData.Add(new StreamContent(avatar.OpenReadStream()), "avatar", avatar.FileName);
        var response = await _http.PostAsync($"api/drive/upload/avatar/{userId}", formData);
        var fileResult = await response.Content.ReadAsJsonAsync<ApiResponse<string>>();
        var updatedAvatarUrl = await _userService.UpdateAvatar(userId, fileResult.Result);
        return updatedAvatarUrl.CreateSuccessResponse("User profile avatar updated.");
      }
      catch (Exception exception)
      {
        return BadRequest(exception.CreateErrorResponse());
      }
    }
  }
}