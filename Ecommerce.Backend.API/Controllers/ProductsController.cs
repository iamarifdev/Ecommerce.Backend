using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Ecommerce.Backend.API.DTO;
using Ecommerce.Backend.API.Helpers;
using Ecommerce.Backend.Common.Helpers;
using Ecommerce.Backend.Common.Models;
using Ecommerce.Backend.Entities;
using Ecommerce.Backend.Services.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Backend.API.Controllers
{
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
    public async Task<ActionResult<ApiResponse<PagedList<Product>>>> GatPagedProductList([FromQuery] PagedQuery query)
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

    // /// <summary>
    // /// Get Product by ID
    // /// </summary>
    // /// <param name="id"></param>
    // [HttpGet("id/{id}")]
    // public async Task<ActionResult<ApiResponse<Product>>> Get(String id)
    // {
    //     try
    //     {
    //         var product = await _productService.GetProductById(id);
    //         return product.CreateSuccessResponse();
    //     }
    //     catch (Exception exception)
    //     {
    //         return BadRequest(exception.CreateErrorResponse());
    //     }
    // }

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

    // /// <summary>
    // /// Update a product by Id
    // /// </summary>
    // /// <param name="id"></param>
    // [HttpPut("update/{id}")]
    // public async Task<ActionResult<ApiResponse<Product>>> Update(string id, UpdateProductDto product)
    // {
    //     try
    //     {
    //         var updatedProduct = await _productService.UpdateProductById(id, _mapper.Map<Product>(product));
    //         return updatedProduct.CreateSuccessResponse("Product updated successfully!");
    //     }
    //     catch (Exception exception)
    //     {
    //         return BadRequest(exception.CreateErrorResponse());
    //     }
    // }

    // /// <summary>
    // /// Activate a product by Id
    // /// </summary>
    // /// <param name="id"></param>
    // [HttpPut("activate/{id}")]
    // public async Task<ActionResult<ApiResponse<Product>>> ToggleActivation(string id, ActivateDto activateDto)
    // {
    //     try
    //     {
    //         var updatedProduct = await _productService.ToggleActivationById(id, activateDto.Status);
    //         var status = activateDto.Status ? "activated" : "deactivated";
    //         return updatedProduct.CreateSuccessResponse($"Product {status} successfully!");
    //     }
    //     catch (Exception exception)
    //     {
    //         return BadRequest(exception.CreateErrorResponse());
    //     }
    // }

    // /// <summary>
    // /// Delete a product by Id
    // /// </summary>
    // /// <param name="id"></param>
    // [HttpDelete("delete/{id}")]
    // public async Task<ActionResult<ApiResponse<Product>>> Delete(string id)
    // {
    //     try
    //     {
    //         var updatedProduct = await _productService.RemoveProductById(id);
    //         return updatedProduct.CreateSuccessResponse("Product deleted successfully!");
    //     }
    //     catch (Exception exception)
    //     {
    //         return BadRequest(exception.CreateErrorResponse());
    //     }
    // }

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
    /// Product feature image upload
    /// </summary>
    /// <param name="productId"></param>
    [HttpPost("{productId}/upload/images")]
    public async Task<ActionResult<ApiResponse<List<string>>>> UploadImages(string productId, List<IFormFile> images)
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
        await _productService.UpdateImages(productId, fileResult.Result);
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