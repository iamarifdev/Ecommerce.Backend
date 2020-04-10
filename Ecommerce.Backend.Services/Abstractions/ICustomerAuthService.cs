using System.Threading.Tasks;
using Ecommerce.Backend.Common.DTO;
using Ecommerce.Backend.Entities;

namespace Ecommerce.Backend.Services.Abstractions
{
  public interface ICustomerAuthService : IBaseService<Customer>
  {
    string GenerateAccessToken(Customer customer);
    string GenerateRefreshToken(Customer customer);
    Task StoreCustomerLogin(string customerId, string accessToken, string refreshToken, RefreshTokenDto oldToken = null);
    Task<RefreshTokenDto> RefreshAccessToken(RefreshTokenDto dto);
    Task<AuthUserDto> Authenticate(string phoneNo, string password);
    Task<bool> LogOut(string phoneNo, string refreshToken);
    Task < (bool, int) > LogOutFromAllDevice(string phoneNo, string refreshToken);
  }
}