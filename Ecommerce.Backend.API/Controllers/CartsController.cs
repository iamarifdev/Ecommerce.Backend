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
  [Route("api/carts")]
  [ApiController]
  public class CartsController : ControllerBase
  {
    private readonly IMapper _mapper;
    private readonly ICartService _cartService;
    public CartsController(ICartService cartService, IMapper mapper)
    {
      _mapper = mapper;
      _cartService = cartService;
    }

    /// <summary>
    /// Get Pagniated Carts
    /// </summary>
    [HttpGet("list")]
    public async Task<ActionResult<ApiResponse<PagedList<Cart>>>> GatPagedProductList([FromQuery] PagedQuery query)
    {
      try
      {
        var pagedCartList = await _cartService.GetPaginatedCarts(query);
        return pagedCartList.CreateSuccessResponse();
      }
      catch (Exception exception)
      {
        return BadRequest(exception.CreateErrorResponse());
      }
    }

    /// <summary>
    /// Get Cart by ID
    /// </summary>
    [HttpGet("id")]
    public async Task<ActionResult<ApiResponse<Cart>>> Get([FromQuery] string cartId, [FromQuery] string customerId)
    {
      try
      {
        var cart = await _cartService.GetCartById(cartId, customerId);
        if (cart == null) throw new Exception("No cart information is found");
        return cart.CreateSuccessResponse();
      }
      catch (Exception exception)
      {
        return BadRequest(exception.CreateErrorResponse());
      }
    }

    // /// <summary>
    // /// Add a new cart
    // /// </summary>
    // [HttpPost("add")]
    // public async Task<ActionResult<ApiResponse<Cart>>> Add(CartDto cartDto)
    // {
    //   try
    //   {
    //     var cart = _mapper.Map<Cart>(cartDto);
    //     var createdCart = await _cartService.AddCart(cart);
    //     return createdCart.CreateSuccessResponse("Cart created successfully!");
    //   }
    //   catch (Exception exception)
    //   {
    //     return BadRequest(exception.CreateErrorResponse());
    //   }
    // }

    /// <summary>
    /// Add a new cart product
    /// </summary>
    [HttpPost("add/product")]
    public async Task<ActionResult<ApiResponse<Cart>>> AddProduct(AddCartProductDto dto)
    {
      try
      {
        var createdCart = await _cartService.AddCartProduct(dto);
        return createdCart.CreateSuccessResponse("Cart created successfully!");
      }
      catch (Exception exception)
      {
        return BadRequest(exception.CreateErrorResponse());
      }
    }

    /// <summary>
    /// Update Cart by ID
    /// </summary>
    [HttpPut("update/{cartId}")]
    public async Task<ActionResult<ApiResponse<Cart>>> Update(string cartId, CartDto cartDto)
    {
      try
      {
        var cart = _mapper.Map<Cart>(cartDto);
        cart = await _cartService.UpdateCartById(cartId, cart);
        return cart.CreateSuccessResponse("Cart updated successfully!");
      }
      catch (Exception exception)
      {
        return BadRequest(exception.CreateErrorResponse());
      }
    }
  }
}