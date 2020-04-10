namespace Ecommerce.Backend.Common.DTO
{
  public class RefreshTokenDto
  {
    public string UserId { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
  }
}