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
using Microsoft.AspNetCore.Http;
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

    /// <summary>
    /// Add a new product
    /// </summary>
    [HttpPost("add")]
    public async Task<ActionResult<ApiResponse<Product>>> Add(ProductAddDto productAddDto)
    {
      try
      {
        var product = _mapper.Map<Product>(productAddDto);
        var createdProduct = await _productService.AddProduct(product);
        return createdProduct.CreateSuccessResponse("Product created successfully!");
      }
      catch (Exception exception)
      {
        return BadRequest(exception.CreateErrorResponse());
      }
    }

    /// <summary>
    /// Update a product by Id
    /// </summary>
    /// <param name="productId"></param>
    [HttpPut("update/{productId}")]
    public async Task<ActionResult<ApiResponse<Product>>> Update(string productId, ProductUpdateDto dto)
    {
      try
      {
        var mappedProduct = _mapper.Map<Product>(dto);
        var updatedProduct = await _productService.UpdateProductById(productId, mappedProduct);
        return updatedProduct.CreateSuccessResponse("Product updated successfully!");
      }
      catch (Exception exception)
      {
        return BadRequest(exception.CreateErrorResponse());
      }
    }

    /// <summary>
    /// Activate a product by Id
    /// </summary>
    /// <param name="productId"></param>
    [HttpPut("activate/{productId}")]
    public async Task<ActionResult<ApiResponse<Product>>> ToggleActivation(string productId, ActivateDto activateDto)
    {
      try
      {
        var updatedProduct = await _productService.ToggleActivationById(productId, activateDto.Status);
        var status = activateDto.Status ? "activated" : "deactivated";
        return updatedProduct.CreateSuccessResponse($"Product {status} successfully!");
      }
      catch (Exception exception)
      {
        return BadRequest(exception.CreateErrorResponse());
      }
    }

    /// <summary>
    /// Delete a product by Id
    /// </summary>
    /// <param name="productId"></param>
    [HttpDelete("delete/{productId}")]
    public async Task<ActionResult<ApiResponse<Product>>> Delete(string productId)
    {
      try
      {
        var deletedProduct = await _productService.RemoveProductById(productId);
        return deletedProduct.CreateSuccessResponse("Product deleted successfully!");
      }
      catch (Exception exception)
      {
        return BadRequest(exception.CreateErrorResponse());
      }
    }

    /// <summary>
    /// Product feature image upload
    /// </summary>
    /// <param name="productId"></param>
    [HttpPost("{productId}/upload/feature-image")]
    public async Task<ActionResult<ApiResponse<string>>> UploadFeatureImage(string productId, IFormFile featureImage)
    {
      try
      {
        var formData = new MultipartFormDataContent();
        formData.Add(new StreamContent(featureImage.OpenReadStream()), "featureImage", featureImage.FileName);
        var response = await _http.PostAsync($"api/drive/products/{productId}/upload/featured-image", formData);
        var fileResult = await response.Content.ReadAsJsonAsync<ApiResponse<string>>();
        await _productService.UpdateFeatureImage(productId, fileResult.Result);
        return fileResult.Result.CreateSuccessResponse("Product featured image updated.");
      }
      catch (Exception exception)
      {
        return BadRequest(exception.CreateErrorResponse());
      }
    }

    /// <summary>
    /// Product color images upload
    /// </summary>
    /// <param name="productId"></param>
    /// <param name="color"></param>
    [HttpPost("{productId}/upload/images/{color}")]
    public async Task<ActionResult<ApiResponse<List<string>>>> UploadImages(string productId, string color, List<IFormFile> images)
    {
      try
      {
        var formData = new MultipartFormDataContent();
        images.ForEach(image =>
        {
          formData.Add(new StreamContent(image.OpenReadStream()), "images", image.FileName);
        });
        var response = await _http.PostAsync($"api/drive/products/{productId}/upload/images", formData);
        var fileResult = await response.Content.ReadAsJsonAsync<ApiResponse<List<string>>>();
        await _productService.UpdateImages(productId, color, fileResult.Result);
        return fileResult.Result.CreateSuccessResponse("Product featured image updated.");
      }
      catch (Exception exception)
      {
        return BadRequest(exception.CreateErrorResponse());
      }
    }

    // /// <summary>
    // /// Product validation
    // /// </summary>
    // [HttpPost("validation")]
    // public async Task<ActionResult<ApiResponse<IdentityDto>>> ValidateProduct(Dictionary<string, string> keyValues)
    // {
    //     try
    //     {
    //         var identity = await _productService.CheckProductAvailibility(keyValues);
    //         return identity.CreateSuccessResponse();
    //     }
    //     catch (Exception exception)
    //     {
    //         return BadRequest(exception.CreateErrorResponse());
    //     }
    // }
  }
}