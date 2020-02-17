using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Ecommerce.Backend.API.DTO
{
  public class ManufactureDetailDto
  {
    [Required]
    public string ModelNo { get; set; }
    public DateTime ReleaseDate { get; set; }
  }

  public class PricingDto
  {
    [Required]
    public double Price { get; set; }
  }

  public class ShippingDetailDto
  {
    public double? Weight { get; set; }
    public double? Width { get; set; }
    public double? Height { get; set; }
    public double? Depth { get; set; }
    public double? Size { get; set; }
  }
  public class ProductAddDto
  {
    [Required]
    public string SKU { get; set; }

    [Required]
    public double InStock { get; set; }
    public bool Availibility { get; set; } = true;

    [Required]
    public string Title { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    public ManufactureDetailDto ManufactureDetail { get; set; }

    [Required]
    public ShippingDetailDto ShippingDetail { get; set; }

    [Required]
    public PricingDto Pricing { get; set; }

    [Required]
    public List<string> Colors { get; set; }

    // [Required]
    // public IFormFile FeatureImage { get; set; }
    // public List<FormFile> Images { get; set; }
  }
}