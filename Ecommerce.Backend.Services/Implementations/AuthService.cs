using System;
using System.Collections.Generic;
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
using MongoDB.Driver;
using MongoDB.Entities;

namespace Ecommerce.Backend.Services.Implementations
{
  public class AuthService
  {
    private readonly TokenStoreDbContext _dbContext;
    private readonly IUserService _userService;
    private readonly IRoleService _roleService;
    private readonly IJwtConfig _jwtConfig;

    public AuthService(
      TokenStoreDbContext dbContext,
      IRoleService roleService,
      IUserService userService,
      IJwtConfig jwtConfig
    )
    {
      _dbContext = dbContext;
      _roleService = roleService;
      _userService = userService;
      _jwtConfig = jwtConfig;
    }

    private string GenerateToken(User user, int days, string secretKey)
    {
      var claims = new Claim[]
      {
        new Claim(ClaimTypes.NameIdentifier, user.ID),
        new Claim(ClaimTypes.Name, user.Username)
      };
      var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
      var signingcredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);
      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(claims),
        Expires = DateTime.Now.AddDays(days),
        SigningCredentials = signingcredentials
      };
      var tokenHandler = new JwtSecurityTokenHandler();
      var securityToken = tokenHandler.CreateToken(tokenDescriptor);
      var token = tokenHandler.WriteToken(securityToken);
      return token;
    }

    private bool RevokeRefreshToken(string userId, string refreshToken)
    {
      var userLogin = _dbContext.UserLogins.FirstOrDefault(
        login => login.UserId == userId && login.RefreshToken == refreshToken
      );
      if (userLogin.IsNotEmpty())
      {
        _dbContext.UserLogins.Remove(userLogin);
        return true;
      }
      return false;
    }

    private(bool, int) RevokeAllRefreshToken(string userId, string refreshToken)
    {
      var userLogin = _dbContext.UserLogins.FirstOrDefault(
        login => login.UserId == userId && login.RefreshToken == refreshToken
      );
      if (userLogin.IsNotEmpty())
      {
        var loggedInDevices = _dbContext.UserLogins.Where(login => login.UserId == userLogin.UserId).ToList();
        _dbContext.UserLogins.RemoveRange(loggedInDevices);
        return (true, loggedInDevices.Count);
      }
      return (false, 0);
    }

    public async Task < (Role, User) > SeedDatabase()
    {
      using(var transaction = new Transaction())
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
              Password = "123456",
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
            newUser.RoleRef.ID = role.ID;
            user = await _userService.Add(newUser);
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
        var existingUserLogin = _dbContext.UserLogins.FirstOrDefault(x =>
          x.AccessToken == oldToken.AccessToken &&
          x.RefreshToken == oldToken.RefreshToken &&
          x.UserId == oldToken.UserId
        );
        if (existingUserLogin.IsNotEmpty())
        {
          existingUserLogin.AccessToken = accessToken;
          existingUserLogin.RefreshToken = refreshToken;
          existingUserLogin.UpdateAt = DateTime.Now;
          _dbContext.UserLogins.Update(existingUserLogin);
          await _dbContext.SaveChangesAsync();
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
        await _dbContext.UserLogins.AddAsync(userLogin);
        await _dbContext.SaveChangesAsync();
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
      return RevokeRefreshToken(user.ID, refreshToken);
    }

    public async Task < (bool, int) > LogOutFromAllDevice(string username, string refreshToken)
    {
      var countLoggedInDevice = 0;
      var user = await _userService.GetByExpression(u => u.Username.ToLower() == username.ToLower());
      if (user.IsEmpty()) return (false, countLoggedInDevice);
      var loggedOutStatus = RevokeAllRefreshToken(user.ID, refreshToken);
      return loggedOutStatus;
    }
  }
}