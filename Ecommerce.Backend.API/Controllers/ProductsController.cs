using System;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Ecommerce.Backend.API.Helpers;
using Ecommerce.Backend.Common.DTO;
using Ecommerce.Backend.Common.Helpers;
using Ecommerce.Backend.Common.Models;
using Ecommerce.Backend.Entities;
using Ecommerce.Backend.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Ecommerce.Backend.API.Controllers
{
  [SwaggerTag("Products")]
  [Produces("application/json")]
  [Route("api/products")]
  [ApiController]
  public class ProductsController : ControllerBase
  {
    private readonly IMapper _mapper;
    private readonly HttpClient _http;
    private readonly IProductService _productService;
    public ProductsController(IProductService productService, IMapper mapper, EcommerceHttpClient httpClient)
    {
      _mapper = mapper;
      _http = httpClient.Http;
      _productService = productService;
    }

    /// <summary>
    /// Get Pagniated Products
    /// </summary>
    [HttpGet("list")]
    public async Task<ActionResult<ApiResponse<PagedList<ProductListItemDto>>>> GatPagedProductList([FromQuery] PagedQuery query)
    {
      try
      {
        var pagedProductList = await _productService.GetPaginatedProducts(query);
        return pagedProductList.CreateSuccessResponse();
      }
      catch (Exception exception)
      {
        return BadRequest(exception.CreateErrorResponse());
      }
    }

    /// <summary>
    /// Get Product by ID
    /// </summary>
    /// <param name="productId"></param>
    [HttpGet("{productId}")]
    public async Task<ActionResult<ApiResponse<Product>>> Get(String productId)
    {
      try
      {
        var product = await _productService.GetProductById(productId);
        return product.CreateSuccessResponse();
      }
      catch (Exception exception)
      {
        return BadRequest(exception.CreateErrorResponse());
      }
    }
  }
}