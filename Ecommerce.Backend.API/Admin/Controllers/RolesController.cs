using System;
using System.Threading.Tasks;
using AutoMapper;
using Ecommerce.Backend.API.Helpers;
using Ecommerce.Backend.Common.DTO;
using Ecommerce.Backend.Common.Models;
using Ecommerce.Backend.Entities;
using Ecommerce.Backend.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Ecommerce.Backend.API.Admin.Controllers
{
  [SwaggerTag("Admin Roles")]
  [Produces("application/json")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [Authorize]
  [Route("admin/api/roles")]
  [ApiController]
  public class RolesController : ControllerBase
  {
    private readonly IMapper _mapper;
    private readonly IRoleService _roleService;
    public RolesController(IRoleService roleService, IMapper mapper)
    {
      _mapper = mapper;
      _roleService = roleService;
    }

    /// <summary>
    /// Get Pagniated roles
    /// </summary>
    [HttpGet("list")]
    public async Task<ActionResult<ApiResponse<PagedList<Role>>>> GatPagedRoleList([FromQuery] PagedQuery query)
    {
      try
      {
        var pagedRoleList = await _roleService.GetPaginatedList(query);
        return pagedRoleList.CreateSuccessResponse();
      }
      catch (Exception exception)
      {
        return BadRequest(exception.CreateErrorResponse());
      }
    }

    /// <summary>
    /// Get role by Id
    /// </summary>
    /// <param name="roleId"></param>
    [HttpGet("{roleId}")]
    public async Task<ActionResult<ApiResponse<Role>>> Get(String roleId)
    {
      try
      {
        var role = await _roleService.GetById(roleId);
        return role.CreateSuccessResponse();
      }
      catch (Exception exception)
      {
        return BadRequest(exception.CreateErrorResponse());
      }
    }

    /// <summary>
    /// Add a new role
    /// </summary>
    [HttpPost("add")]
    public async Task<ActionResult<ApiResponse<Role>>> Add(RoleAddDto dto)
    {
      try
      {
        var role = _mapper.Map<Role>(dto);
        var createdRole = await _roleService.Add(role);
        return createdRole.CreateSuccessResponse("Role created successfully!");
      }
      catch (Exception exception)
      {
        return BadRequest(exception.CreateErrorResponse());
      }
    }

    /// <summary>
    /// Update a role by Id
    /// </summary>
    /// <param name="roleId"></param>
    [HttpPut("update/{roleId}")]
    public async Task<ActionResult<ApiResponse<Role>>> Update(string roleId, Role role)
    {
      try
      {
        var updatedRole = await _roleService.UpdateById(roleId, role);
        return updatedRole.CreateSuccessResponse("Role updated successfully!");
      }
      catch (Exception exception)
      {
        return BadRequest(exception.CreateErrorResponse());
      }
    }

    /// <summary>
    /// Activate a role by Id
    /// </summary>
    /// <param name="roleId"></param>
    [HttpPut("activate/{roleId}")]
    public async Task<ActionResult<ApiResponse<Role>>> ToggleActivation(string roleId, ActivateDto activateDto)
    {
      try
      {
        var updatedRole = await _roleService.ToggleActivationById(roleId, activateDto.Status);
        var status = activateDto.Status ? "activated" : "deactivated";
        return updatedRole.CreateSuccessResponse($"Role {status} successfully!");
      }
      catch (Exception exception)
      {
        return BadRequest(exception.CreateErrorResponse());
      }
    }

    /// <summary>
    /// Delete a role by Id
    /// </summary>
    /// <param name="roleId"></param>
    [HttpDelete("delete/{roleId}")]
    public async Task<ActionResult<ApiResponse<Role>>> Delete(string roleId)
    {
      try
      {
        var deletedRole = await _roleService.RemoveById(roleId);
        return deletedRole.CreateSuccessResponse("Role deleted successfully!");
      }
      catch (Exception exception)
      {
        return BadRequest(exception.CreateErrorResponse());
      }
    }
  }
}
