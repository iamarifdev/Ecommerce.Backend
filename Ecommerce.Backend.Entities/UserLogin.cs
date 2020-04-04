using System;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Backend.Entities
{
  public class UserLogin
  {
    [Required]
    public int UserLoginId { get; set; }

    [Required]
    public string UserId { get; set; }

    [Required]
    public string AccessToken { get; set; }

    [Required]
    public string RefreshToken { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdateAt { get; set; } = DateTime.Now;
  }
}