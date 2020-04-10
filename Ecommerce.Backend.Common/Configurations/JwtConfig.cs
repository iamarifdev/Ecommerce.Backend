namespace Ecommerce.Backend.Common.Configurations
{
  public class JwtConfig : IJwtConfig
  {
    public JwtConfig Customer { get; set; }
    public string AccessTokenSecretKey { get; set; }
    public string RefreshTokenSecretKey { get; set; }
    public int AccessTokenExpiresIn { get; set; }
    public int RefreshTokenExpiresIn { get; set; }
  }

  public interface IJwtConfig
  {
    JwtConfig Customer { get; set; }
    string AccessTokenSecretKey { get; set; }
    string RefreshTokenSecretKey { get; set; }
    int AccessTokenExpiresIn { get; set; }
    int RefreshTokenExpiresIn { get; set; }
  }
}