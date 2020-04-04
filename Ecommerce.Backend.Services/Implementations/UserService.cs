using System.Threading.Tasks;
using Ecommerce.Backend.Common.Helpers;
using Ecommerce.Backend.Entities;
using Ecommerce.Backend.Services.Abstractions;
using MongoDB.Driver;

namespace Ecommerce.Backend.Services.Implementations
{
  public class UserService : BaseService<User>, IUserService
  {
    /// <summary>
    /// Use it only when seeding database
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public async Task<User> AddUser(User user)
    {
      var salt = Salt.Create();
      var passwordHash = Hash.Create(user.Password, salt);
      user.Password = passwordHash;
      user.Salt = salt;
      user.IsEnabled = true;
      await Add(user);
      return user;
    }
    public async Task<string> UpdateAvatar(string userId, string avatarUrl)
    {
      var update = Builders<User>.Update.Set("FeatureImageUrl", avatarUrl);
      await UpdatePartial(r => r.ID == userId, update);
      return avatarUrl;
    }
  }
}