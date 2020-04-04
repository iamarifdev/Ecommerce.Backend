using System.Threading.Tasks;
using Ecommerce.Backend.Entities;
using Ecommerce.Backend.Services.Abstractions;
using MongoDB.Driver;

namespace Ecommerce.Backend.Services.Implementations
{
  public class UserService : BaseService<User>, IUserService
  {
    public async Task<string> UpdateAvatar(string userId, string avatarUrl)
    {
      var update = Builders<User>.Update.Set("FeatureImageUrl", avatarUrl);
      await UpdatePartial(r => r.ID == userId, update);
      return avatarUrl;
    }
  }
}