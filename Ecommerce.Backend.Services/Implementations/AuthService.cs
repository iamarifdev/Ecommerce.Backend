using System;
using System.Collections.Generic;
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
using MongoDB.Entities;

namespace Ecommerce.Backend.Services.Implementations
{
  public class AuthService : IAuthService
  {
    private readonly IUserService _userService;
    private readonly IUserLoginService _userLoginService;
    private readonly IRoleService _roleService;
    private readonly IJwtConfig _jwtConfig;

    public AuthService(
      IUserLoginService userLoginService,
      IRoleService roleService,
      IUserService userService,
      IJwtConfig jwtConfig
    )
    {
      _userLoginService = userLoginService;
      _roleService = roleService;
      _userService = userService;
      _jwtConfig = jwtConfig;
    }

    private string GenerateToken(User user, double minutes, string secretKey)
    {
      var claims = new Claim[]
      {
        new Claim(ClaimTypes.NameIdentifier, user.ID),
        new Claim(ClaimTypes.Name, user.Username),
        new Claim(ClaimTypes.Role, user.RoleRef.ID)
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

    private async Task<bool> RevokeRefreshToken(string userId, string refreshToken)
    {
      var userLogin = await _userLoginService.GetByExpression(
        login => login.UserId == userId && login.RefreshToken == refreshToken
      );
      if (userLogin.IsNotEmpty())
      {
        await _userLoginService.DeleteById(userLogin.ID);
        return true;
      }
      return false;
    }

    private async Task<(bool, int)> RevokeAllRefreshToken(string userId, string refreshToken)
    {
      var userLogin = await _userLoginService.GetByExpression(
        login => login.UserId == userId && login.RefreshToken == refreshToken
      );
      if (userLogin.IsNotEmpty())
      {
        var loggedInDevices = await _userLoginService.GetAllByExpression(login => login.UserId == userLogin.UserId);
        await _userLoginService.DeleteByExpression(login => login.UserId == userLogin.UserId);
        return (true, loggedInDevices.Count());
      }
      return (false, 0);
    }

    public async Task<(Role, User)> SeedDatabase()
    {
      using (var transaction = new Transaction())
      {
        try
        {
          var username = "superadmin";
          var rolename = "Super Admin";

          var role = await _roleService.GetByExpression(role => role.Name == rolename);
          var user = await _userService.GetByExpression(user => user.Username == username);

          if (role.IsEmpty() && user.IsEmpty())
          {
            role = await _roleService.Add(new Role
            {
              Name = rolename,
              Description = "A superadmin user with all previledge"
            });

            var newUser = new User
            {
              FullName = "Ariful Islam",
              Username = username,
              RoleRef = new One<Role> { ID = role.ID },
              Password = "Admin123456$",
              Email = "arifjmamun24@gmail.com",
              Remarks = "A superadmin user",
              Addresses = new List<UserAddress>
              {
                new UserAddress
                {
                  Description = "Charshapmari, Shapmari",
                  District = "Sherpur",
                  Thana = "Sherpur",
                  PostCode = "2100"
                }
              },
              PhoneNumbers = new List<UserPhoneNumber>
              {
                new UserPhoneNumber { PhoneNo = "01793574440" }
              }
            };
            user = await _userService.AddUser(newUser);
            await transaction.CommitAsync();
            return (role, user);
          }
          else
          {
            await transaction.AbortAsync();
            return (role, user);
          }
        }
        catch (Exception exception)
        {
          await transaction.AbortAsync();
          throw exception;
        }
      }
    }

    public string GenerateAccessToken(User user)
    {
      var token = GenerateToken(user, _jwtConfig.AccessTokenExpiresIn, _jwtConfig.AccessTokenSecretKey);
      return token;
    }

    public string GenerateRefreshToken(User user)
    {
      var token = GenerateToken(user, _jwtConfig.RefreshTokenExpiresIn, _jwtConfig.RefreshTokenSecretKey);
      return token;
    }

    public async Task StoreUserLogin(string userId, string accessToken, string refreshToken, RefreshTokenDto oldToken = null)
    {
      if (oldToken.IsNotEmpty())
      {
        Expression<Func<UserLogin, bool>> expression = (x) =>
          x.AccessToken == oldToken.AccessToken &&
          x.RefreshToken == oldToken.RefreshToken &&
          x.UserId == oldToken.UserId;
        var existingUserLogin = await _userLoginService.GetByExpression(expression);
        if (existingUserLogin.IsNotEmpty())
        {
          var update = Builders<UserLogin>.Update
            .Set(cart => cart.AccessToken, accessToken)
            .Set(cart => cart.RefreshToken, refreshToken)
            .Set(cart => cart.UpdatedAt, DateTime.Now);
          await _userLoginService.UpdatePartial(expression, update);
        }
      }
      else
      {
        var userLogin = new UserLogin
        {
          UserId = userId,
          AccessToken = accessToken,
          RefreshToken = refreshToken
        };
        await _userLoginService.Add(userLogin);
      }
    }

    public async Task<RefreshTokenDto> RefreshAccessToken(RefreshTokenDto dto)
    {
      var user = await _userService.GetById(dto.UserId);
      var accessToken = GenerateAccessToken(user);
      var refreshToken = GenerateRefreshToken(user);

      var securityToken = new RefreshTokenDto
      {
        AccessToken = accessToken,
        RefreshToken = refreshToken,
        UserId = user.ID
      };
      await StoreUserLogin(user.ID, accessToken, refreshToken, dto);
      return securityToken;
    }

    public async Task<AuthUserDto> Authenticate(string username, string password)
    {
      var user = await _userService.GetByExpression(x => x.Username.ToLower() == username.ToLower());
      if (user.IsEmpty()) return null;

      var isValidCredential = Hash.Validate(password, user.Salt, user.Password);
      if (!isValidCredential) return null;

      var authUser = new AuthUserDto
      {
        UserId = user.ID,
        Email = user.Email,
        Role = new UserRole(user.RoleRef),
        Username = user.Username,
        FullName = user.FullName,
        AvatarUrl = user.AvatarUrl,
        AccessToken = GenerateAccessToken(user),
        RefreshToken = GenerateRefreshToken(user),
      };
      await StoreUserLogin(authUser.UserId, authUser.AccessToken, authUser.RefreshToken);
      return authUser;
    }

    public async Task<bool> LogOut(string username, string refreshToken)
    {
      var user = await _userService.GetByExpression(u => u.Username.ToLower() == username.ToLower());
      if (user.IsEmpty()) return false;
      return await RevokeRefreshToken(user.ID, refreshToken);
    }

    public async Task<(bool, int)> LogOutFromAllDevice(string username, string refreshToken)
    {
      var countLoggedInDevice = 0;
      var user = await _userService.GetByExpression(u => u.Username.ToLower() == username.ToLower());
      if (user.IsEmpty()) return (false, countLoggedInDevice);
      var loggedOutStatus = await RevokeAllRefreshToken(user.ID, refreshToken);
      return loggedOutStatus;
    }
  }
}