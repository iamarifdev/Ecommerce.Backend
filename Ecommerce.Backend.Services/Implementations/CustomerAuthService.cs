using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.Backend.Common.Configurations;
using Ecommerce.Backend.Common.DTO;
using Ecommerce.Backend.Common.Helpers;
using Ecommerce.Backend.Entities;
using Ecommerce.Backend.Services.Abstractions;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;

namespace Ecommerce.Backend.Services.Implementations
{
  public class CustomerAuthService : BaseService<Customer>, ICustomerAuthService
  {
    private readonly ICustomerLoginService _customerLoginService;
    private readonly IJwtConfig _jwtConfig;

    public CustomerAuthService(ICustomerLoginService customerLoginService, IJwtConfig jwtConfig)
    {
      _customerLoginService = customerLoginService;
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

    private async Task<bool> RevokeRefreshToken(string customerId, string refreshToken)
    {
      var customerLogin = await _customerLoginService.GetByExpression(
        login => login.CustomerId == customerId && login.RefreshToken == refreshToken
      );
      if (customerLogin.IsNotEmpty())
      {
        await _customerLoginService.DeleteById(customerLogin.ID);
        return true;
      }
      return false;
    }

    private async Task<(bool, int)> RevokeAllRefreshToken(string customerId, string refreshToken)
    {
      var userLogin = await _customerLoginService.GetByExpression(
        login => login.CustomerId == customerId && login.RefreshToken == refreshToken
      );
      if (userLogin.IsNotEmpty())
      {
        var loggedInDevices = await _customerLoginService.GetAllByExpression(login => login.CustomerId == userLogin.CustomerId);
        await _customerLoginService.DeleteByExpression(login => login.CustomerId == userLogin.CustomerId);
        return (true, loggedInDevices.Count());
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
        Expression<Func<CustomerLogin, bool>> expression = (x) =>
         x.AccessToken == oldToken.AccessToken &&
         x.RefreshToken == oldToken.RefreshToken &&
         x.CustomerId == oldToken.UserId;
        var existingCustomerLogin = await _customerLoginService.GetByExpression(expression);
        if (existingCustomerLogin.IsNotEmpty())
        {
          var update = Builders<CustomerLogin>.Update
           .Set(cart => cart.AccessToken, accessToken)
           .Set(cart => cart.RefreshToken, refreshToken)
           .Set(cart => cart.UpdatedAt, DateTime.Now);
          await _customerLoginService.UpdatePartial(expression, update);
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
        await _customerLoginService.Add(customerLogin);
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
        PhoneNo = customer.PhoneNo,
        Email = customer.Email,
        Role = null,
        Username = null,
        FullName = customer.FullName,
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
      return await RevokeRefreshToken(customer.ID, refreshToken);
    }

    public async Task<(bool, int)> LogOutFromAllDevice(string phoneNo, string refreshToken)
    {
      var countLoggedInDevice = 0;
      var customer = await GetByExpression(u => u.PhoneNo == phoneNo);
      if (customer.IsEmpty()) return (false, countLoggedInDevice);
      var loggedOutStatus = await RevokeAllRefreshToken(customer.ID, refreshToken);
      return loggedOutStatus;
    }
  }
}