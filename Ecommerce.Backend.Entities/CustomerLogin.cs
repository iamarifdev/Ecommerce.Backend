using System;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Backend.Entities
{
  public class CustomerLogin
  {
    [Required]
    public int CustomerLoginId { get; set; }

    [Required]
    public string CustomerId { get; set; }

    [Required]
    public string AccessToken { get; set; }

    [Required]
    public string RefreshToken { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdateAt { get; set; } = DateTime.Now;
  }
}