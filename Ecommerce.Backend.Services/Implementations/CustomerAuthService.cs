using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.Backend.Common.Configurations;
using Ecommerce.Backend.Common.DTO;
using Ecommerce.Backend.Common.Helpers;
using Ecommerce.Backend.Entities;
using Ecommerce.Backend.Services.Abstractions;
using Microsoft.IdentityModel.Tokens;

namespace Ecommerce.Backend.Services.Implementations
{
  public class CustomerAuthService : BaseService<Customer>, ICustomerAuthService
  {
    private readonly TokenStoreDbContext _dbContext;
    private readonly IJwtConfig _jwtConfig;

    public CustomerAuthService(TokenStoreDbContext dbContext, IJwtConfig jwtConfig)
    {
      _dbContext = dbContext;
      _jwtConfig = jwtConfig;
    }

    private string GenerateToken(Customer customer, double minutes, string secretKey)
    {
      var claims = new Claim[]
      {
        new Claim(ClaimTypes.NameIdentifier, customer.ID),
        new Claim(ClaimTypes.MobilePhone, customer.PhoneNo)
      };
      var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
      var signingcredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);
      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(claims),
        Expires = DateTime.Now.AddMinutes(minutes),
        SigningCredentials = signingcredentials
      };
      var tokenHandler = new JwtSecurityTokenHandler();
      var securityToken = tokenHandler.CreateToken(tokenDescriptor);
      var token = tokenHandler.WriteToken(securityToken);
      return token;
    }

    private bool RevokeRefreshToken(string customerId, string refreshToken)
    {
      var customerLogin = _dbContext.CustomerLogins.FirstOrDefault(
        login => login.CustomerId == customerId && login.RefreshToken == refreshToken
      );
      if (customerLogin.IsNotEmpty())
      {
        _dbContext.CustomerLogins.Remove(customerLogin);
        return true;
      }
      return false;
    }

    private(bool, int) RevokeAllRefreshToken(string customerId, string refreshToken)
    {
      var userLogin = _dbContext.CustomerLogins.FirstOrDefault(
        login => login.CustomerId == customerId && login.RefreshToken == refreshToken
      );
      if (userLogin.IsNotEmpty())
      {
        var loggedInDevices = _dbContext.CustomerLogins.Where(login => login.CustomerId == userLogin.CustomerId).ToList();
        _dbContext.CustomerLogins.RemoveRange(loggedInDevices);
        return (true, loggedInDevices.Count);
      }
      return (false, 0);
    }

    public string GenerateAccessToken(Customer customer)
    {
      var token = GenerateToken(
        customer,
        _jwtConfig.Customer.AccessTokenExpiresIn,
        _jwtConfig.Customer.AccessTokenSecretKey
      );
      return token;
    }

    public string GenerateRefreshToken(Customer customer)
    {
      var token = GenerateToken(
        customer,
        _jwtConfig.Customer.RefreshTokenExpiresIn,
        _jwtConfig.Customer.RefreshTokenSecretKey
      );
      return token;
    }

    public async Task StoreCustomerLogin(string customerId, string accessToken, string refreshToken, RefreshTokenDto oldToken = null)
    {
      if (oldToken.IsNotEmpty())
      {
        var existingCustomerLogin = _dbContext.CustomerLogins.FirstOrDefault(x =>
          x.AccessToken == oldToken.AccessToken &&
          x.RefreshToken == oldToken.RefreshToken &&
          x.CustomerId == oldToken.UserId
        );
        if (existingCustomerLogin.IsNotEmpty())
        {
          existingCustomerLogin.AccessToken = accessToken;
          existingCustomerLogin.RefreshToken = refreshToken;
          existingCustomerLogin.UpdateAt = DateTime.Now;
          _dbContext.CustomerLogins.Update(existingCustomerLogin);
          await _dbContext.SaveChangesAsync();
        }
      }
      else
      {
        var customerLogin = new CustomerLogin
        {
          CustomerId = customerId,
          AccessToken = accessToken,
          RefreshToken = refreshToken
        };
        await _dbContext.CustomerLogins.AddAsync(customerLogin);
        await _dbContext.SaveChangesAsync();
      }
    }

    public async Task<RefreshTokenDto> RefreshAccessToken(RefreshTokenDto dto)
    {
      var customer = await GetById(dto.UserId);
      var accessToken = GenerateAccessToken(customer);
      var refreshToken = GenerateRefreshToken(customer);

      var securityToken = new RefreshTokenDto
      {
        AccessToken = accessToken,
        RefreshToken = refreshToken,
        UserId = customer.ID
      };
      await StoreCustomerLogin(customer.ID, accessToken, refreshToken, dto);
      return securityToken;
    }

    public async Task<AuthUserDto> Authenticate(string phoneNo, string password)
    {
      var customer = await GetByExpression(x => x.PhoneNo == phoneNo);
      if (customer.IsEmpty()) return null;

      var isValidCredential = Hash.Validate(password, customer.Auth.Salt, customer.Auth.Password);
      if (!isValidCredential) return null;

      var authUser = new AuthUserDto
      {
        UserId = customer.ID,
        Role = null,
        Username = null,
        FullName = customer.FirstName.IsNotEmpty() ? $"{customer.FirstName} {customer.LastName}" : null,
        AvatarUrl = customer.AvatarUrl,
        AccessToken = GenerateAccessToken(customer),
        RefreshToken = GenerateRefreshToken(customer),
      };
      await StoreCustomerLogin(authUser.UserId, authUser.AccessToken, authUser.RefreshToken);
      return authUser;
    }

    public async Task<bool> LogOut(string phoneNo, string refreshToken)
    {
      var customer = await GetByExpression(u => u.PhoneNo == phoneNo);
      if (customer.IsEmpty()) return false;
      return RevokeRefreshToken(customer.ID, refreshToken);
    }

    public async Task <(bool, int)> LogOutFromAllDevice(string phoneNo, string refreshToken)
    {
      var countLoggedInDevice = 0;
      var customer = await GetByExpression(u => u.PhoneNo == phoneNo);
      if (customer.IsEmpty()) return (false, countLoggedInDevice);
      var loggedOutStatus = RevokeAllRefreshToken(customer.ID, refreshToken);
      return loggedOutStatus;
    }
  }
}