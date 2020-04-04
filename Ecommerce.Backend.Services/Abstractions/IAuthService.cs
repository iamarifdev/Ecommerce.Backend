using System.Threading.Tasks;
using Ecommerce.Backend.Common.DTO;
using Ecommerce.Backend.Entities;

namespace Ecommerce.Backend.Services.Abstractions
{
  public interface IAuthService
  {
    Task <(Role, User)> SeedDatabase();
    string GenerateAccessToken(User user);
    string GenerateRefreshToken(User user);
    Task StoreUserLogin(string userId, string accessToken, string refreshToken, RefreshTokenDto oldToken = null);
    Task<RefreshTokenDto> RefreshAccessToken(RefreshTokenDto dto);
    Task<AuthUserDto> Authenticate(string username, string password);
    Task<bool> LogOut(string username, string refreshToken);
    Task <(bool, int)> LogOutFromAllDevice(string username, string refreshToken);
  }
}