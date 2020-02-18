using System;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Ecommerce.Backend.API.DTO;
using Ecommerce.Backend.API.Helpers;
using Ecommerce.Backend.Common.Configurations;
using Ecommerce.Backend.Common.Models;
using Ecommerce.Backend.Entities;
using Ecommerce.Backend.Services.Abstractions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Backend.API.Controllers
{
  [Route("api/products")]
  [ApiController]
  public class ProductsController : ControllerBase
  {
    private readonly IProductService _productService;
    private readonly IMapper _mapper;
    private readonly IWebHostEnvironment _environment;
    private readonly IAppConfig _appConfig;
    public ProductsController(IProductService productService, IMapper mapper, IWebHostEnvironment environment, IAppConfig appConfig)
    {
      _productService = productService;
      _mapper = mapper;
      _environment = environment;
      _appConfig = appConfig;
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
        if (featureImage.Length < 1) return BadRequest("Invalid file");
        var folderName = Path.Combine("uploads", _appConfig.ProductDirectory, _appConfig.ProductFeatureImagesDirectory);
        var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
        if (!Directory.Exists(pathToSave)) Directory.CreateDirectory(pathToSave);
        var fileName = $"{productId}_{featureImage.FileName}";
        var fullPath = Path.Combine(pathToSave, fileName);
        // todo change the url have host part
        var featureImageUrl = Path.Combine("\\", folderName, fileName).Replace("\\", "/").Replace("uploads", "assets");
        using(var stream = new FileStream(fullPath, FileMode.Create))
        {
          featureImage.CopyTo(stream);
        }
        await _productService.UpdateFeatureImage(productId, featureImageUrl);
        return featureImageUrl.CreateSuccessResponse("Product feature image updated.");
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