using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Ecommerce.Backend.Common.Helpers;
using Ecommerce.Backend.Common.Models;
using Ecommerce.Backend.Entities;
using Ecommerce.Backend.Services.Abstractions;
using MongoDB.Driver;
using MongoDB.Entities;

namespace Ecommerce.Backend.Services.Implementations
{
  public class ProductService : IProductService
  {
    private readonly IMongoCollection<Product> _products;

    public ProductService()
    {
      _products = DB.Collection<Product>();
    }

    public async Task<PagedList<Product>> GetPaginatedProducts(PagedQuery query)
    {
      Expression<Func<Product, bool>> allConditions = (product) => !product.IsDeleted;
      Expression<Func<Product, bool>> conditions = (product) => !product.IsDeleted && product.IsEnabled;

      var filterConditions = Builders<Product>.Filter.Where(query.All ? allConditions : conditions);

      var count = (int) await _products.CountDocumentsAsync(filterConditions);
      var products = await _products
        .Find(filterConditions)
        // .Project(user => new User
        // {
        //     ID = user.ID,
        //         RoleId = user.RoleId,
        //         Username = user.Username,
        //         FullName = user.FullName,
        //         Email = user.Email,
        //         PhoneNumbers = user.PhoneNumbers,
        //         ContactNo = user.ContactNo,
        //         ContactPerson = user.ContactPerson,
        //         Remarks = user.Remarks,
        //         CreatedAt = user.CreatedAt,
        //         UpdatedAt = user.UpdatedAt,
        //         AvatarUrl = user.AvatarUrl,
        //         IsEnabled = user.IsEnabled
        // })
        .SortBy(user => user.Title)
        .Skip(query.PageSize * (query.Page - 1))
        .Limit(query.PageSize)
        .ToListAsync();
      return products.ToPagedList(count);
    }

    public async Task<Product> AddProduct(Product product)
    {
      await _products.InsertOneAsync(product);
      return product;
    }

    public async Task<Product> UpdateFeatureImage(string productId, string featureImageUrl)
    {
      var update = Builders<Product>.Update
        .Set("FeatureImageUrl", featureImageUrl);
      var updatedProduct = await _products.FindOneAndUpdateAsync(r => r.ID == productId, update);
      return updatedProduct;
    }

    public async Task<Product> UpdateImages(string productId, List<string> imageUrls)
    {
      var update = Builders<Product>.Update
        .Set("Images", imageUrls);
      var updatedProduct = await _products.FindOneAndUpdateAsync(r => r.ID == productId, update);
      return updatedProduct;
    }

  }

  //     public class UserService : IUserService
  //   {
  //     private readonly IMongoCollection<User> _users;
  //     private readonly IDriveConfig _driveConfig;

  //     public UserService(IDriveConfig driveConfig)
  //     {
  //       _driveConfig = driveConfig;
  //       _users = DB.Collection<User>();
  //     }

  //     public async Task<PagedList<User>> GetPaginatedUsers(PagedQuery query)
  //     {
  //       Expression<Func<User, bool>> allConditions = (user) => !user.IsDeleted;
  //       Expression<Func<User, bool>> conditions = (user) => !user.IsDeleted && user.IsEnabled;

  //       var filterConditions = Builders<User>.Filter.Where(query.All ? allConditions : conditions);

  //       var count = (int)await _users.CountDocumentsAsync(filterConditions);
  //       var users = await _users
  //         .Find(filterConditions)
  //         .Project(user => new User
  //         {
  //           ID = user.ID,
  //           RoleId = user.RoleId,
  //           Username = user.Username,
  //           FullName = user.FullName,
  //           Email = user.Email,
  //           PhoneNumbers = user.PhoneNumbers,
  //           ContactNo = user.ContactNo,
  //           ContactPerson = user.ContactPerson,
  //           Remarks = user.Remarks,
  //           CreatedAt = user.CreatedAt,
  //           UpdatedAt = user.UpdatedAt,
  //           AvatarUrl = user.AvatarUrl,
  //           IsEnabled = user.IsEnabled
  //         })
  //         .SortBy(user => user.FullName)
  //         .Skip(query.PageSize * (query.Page - 1))
  //         .Limit(query.PageSize)
  //         .ToListAsync();
  //       return users.ToPagedList(count);
  //     }

  //     public async Task<User> GetUserById(string userId)
  //     {
  //       var user = await _users.FindAsync<User>(x => x.ID == userId).Result.FirstOrDefaultAsync();
  //       return user;
  //     }

  //     public async Task<User> GetUserByUsername(string username)
  //     {
  //       var user = await _users.FindAsync<User>(x => x.Username == username).Result.FirstOrDefaultAsync();
  //       return user;
  //     }

  //     // only for internal use, contains passsword and other security related info
  //     public async Task<User> GetUserByCondition(Expression<Func<User, bool>> condition)
  //     {
  //       var user = await _users.FindAsync<User>(condition).Result.FirstOrDefaultAsync();
  //       return user;
  //     }

  //     public async Task<User> AddUser(User user)
  //     {
  //       var salt = Salt.Create();
  //       var passwordHash = Hash.Create(user.Password, salt);
  //       user.Password = passwordHash;
  //       user.Salt = salt;
  //       await _users.InsertOneAsync(user);
  //       return user;
  //     }

  //     public async Task<User> UpdateUserById(string id, User user)
  //     {
  //       var update = Builders<User>.Update
  //         .Set("FullName", user.FullName)
  //         .Set("Addresses", user.Addresses)
  //         .Set("Email", user.Email)
  //         .Set("Username", user.Username)
  //         .Set("ContactNo", user.ContactNo)
  //         .Set("ContactPerson", user.ContactPerson)
  //         .Set("PhoneNumbers", user.PhoneNumbers)
  //         .Set("Remarks", user.Remarks)
  //         .Set("IsEnabled", user.IsEnabled)
  //         .Set("RoleId", user.RoleId)
  //         // .Set("CompanyId", user.CompanyId)
  //         .Set("UpdatedAt", user.UpdatedAt);
  //       var updatedUser = await _users.FindOneAndUpdateAsync(r => r.ID == id, update);
  //       return updatedUser;
  //     }

  //     public async Task<User> ToggleActivationById(string id, bool status)
  //     {
  //       var update = Builders<User>.Update
  //         .Set("IsEnabled", status)
  //         .Set("UpdatedAt", DateTime.Now);
  //       var updatedUser = await _users.FindOneAndUpdateAsync(x => x.ID == id, update);
  //       return updatedUser;
  //     }

  //     public async Task<User> RemoveUserById(string id)
  //     {
  //       var update = Builders<User>.Update
  //         .Set("IsEnabled", false)
  //         .Set("IsDeleted", true)
  //         .Set("UpdatedAt", DateTime.Now);
  //       var deletedUser = await _users.FindOneAndUpdateAsync(x => x.ID == id, update);
  //       return deletedUser;
  //     }

  //     public async Task<string> UpdateUserAvatar(string id, FileInfo fileInfo)
  //     {
  //       var avatarUrl = _driveConfig.ImageUrlTemplate.Replace("{id}", fileInfo.Id);
  //       var update = Builders<User>.Update
  //         .Set("AvatarUrl", avatarUrl)
  //         .Set("UpdatedAt", DateTime.Now);
  //       await _users.FindOneAndUpdateAsync(r => r.ID == id, update);
  //       return avatarUrl;
  //     }

  //     public async Task<IdentityDto> CheckUserAvailibility(Dictionary<string, string> keyValues)
  //     {
  //       var identity = new IdentityDto();
  //       var filterConditions = "{ ";
  //       if (keyValues.Keys.Any(key => key == "username"))
  //       {
  //         var username = keyValues["username"].ToLower();
  //         filterConditions += $"username: '{username}'";
  //       }
  //       if (keyValues.Keys.Any(key => key == "email"))
  //       {
  //         var email = keyValues["email"].ToLower();
  //         filterConditions += $"email: '{email}'";
  //       }
  //       if (keyValues.Keys.Any(key => key == "phoneNumbers.phoneNo"))
  //       {
  //         var phoneNo = keyValues["phoneNumbers.phoneNo"];
  //         filterConditions += $"'phoneNumbers.phoneNo': '{phoneNo}'";
  //       }
  //       filterConditions += " }";

  //       User currentUser = null;
  //       if (keyValues.Keys.Any(key => key == "userId"))
  //       {
  //         var userId = keyValues["userId"];
  //         currentUser = await GetUserById(userId);
  //       }
  //       var user = await _users.FindAsync(filterConditions).Result.FirstOrDefaultAsync();

  //       if (user != null && currentUser != null)
  //       {
  //         identity.IsValid = user.ID == currentUser.ID;
  //       }
  //       else
  //       {
  //         identity.IsValid = user == null;
  //       }
  //       return identity;
  //     }
  //   }
}