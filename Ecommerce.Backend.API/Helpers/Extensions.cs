using AutoMapper;
using Ecommerce.Backend.API.AutoMappingProfiles;
using Ecommerce.Backend.Common.Configurations;
using Ecommerce.Backend.Common.Helpers;
using Ecommerce.Backend.Entities;
using Ecommerce.Backend.Services.Abstractions;
using Ecommerce.Backend.Services.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Entities;

namespace Ecommerce.Backend.API.Helpers
{
  public static class Extensions
  {
    public static IHost MigrateDatabase<T>(this IHost webHost) where T : DbContext
    {
      var serviceScopeFactory = (IServiceScopeFactory) webHost.Services.GetService(typeof(IServiceScopeFactory));
      using(var scope = serviceScopeFactory.CreateScope())
      {
        var services = scope.ServiceProvider;
        var dbContext = services.GetRequiredService<T>();
        dbContext.Database.Migrate();
      }
      return webHost;
    }

    public static IServiceCollection InitiateDbConnection(this IServiceCollection services, IDatabaseSetting dbSetting)
    {
      switch (dbSetting.Env)
      {
        case "Dev":
          services.AddMongoDBEntities(dbSetting.DatabaseName);
          break;
        case "Prod":
          services.AddMongoDBEntities(dbSetting.DbSettings, dbSetting.DatabaseName);
          break;
      }
      DB.Migrate();
      return services;
    }

    public static IServiceCollection RegisterAutoMappingProfiles(this IServiceCollection services)
    {
      services.AddAutoMapper(config => config.AddProfile(new RoleMappingProfile()), typeof(Startup));
      services.AddAutoMapper(config => config.AddProfile(new UserMappingProfile()), typeof(Startup));
      services.AddAutoMapper(config => config.AddProfile(new ProductMappingProfile()), typeof(Startup));
      services.AddAutoMapper(config => config.AddProfile(new CartMappingProfile()), typeof(Startup));
      services.AddAutoMapper(config => config.AddProfile(new ShippingMethodMappingProfile()), typeof(Startup));
      services.AddAutoMapper(config => config.AddProfile(new PaymentMethodMappingProfile()), typeof(Startup));
      services.AddAutoMapper(config => config.AddProfile(new CustomerMappingProfile()), typeof(Startup));
      return services;
    }

    public static IServiceCollection RegisterAPIServices(this IServiceCollection services)
    {
      services.AddScoped<EcommerceHttpClient>();
      services.AddScoped<IAuthService, AuthService>();
      services.AddScoped<IRoleService, RoleService>();
      services.AddScoped<IUserService, UserService>();
      services.AddScoped<IProductService, ProductService>();
      services.AddScoped<ICartService, CartService>();
      services.AddScoped<IShippingMethodService, ShippingMethodService>();
      services.AddScoped<IPaymentMethodService, PaymentMethodService>();
      services.AddScoped<ICustomerService, CustomerService>();
      services.AddScoped<IOrderService, OrderService>();
      services.AddScoped<ICustomer2FAVerificationService, Customer2FAVerificationService>();
      return services;
    }

    public static IServiceCollection RegisterAllDBIndex(this IServiceCollection services)
    {
      // Customer Index
      DB.Index<Role>()
        .Key(x => x.Name, KeyType.Ascending)
        .Option(o => o.Unique = true)
        .Create();

      // Customer Index
      DB.Index<User>()
        .Key(x => x.Username, KeyType.Ascending)
        .Option(o => o.Unique = true)
        .Create();

      // Customer Index
      DB.Index<User>()
        .Key(x => x.Email, KeyType.Ascending)
        .Option(o => o.Unique = true)
        .Create();

      // Product Index
      DB.Index<Product>()
        .Key(x => x.Title, KeyType.Ascending)
        .Option(o => o.Unique = true)
        .Create();
      DB.Index<Product>()
        .Key(x => x.SKU, KeyType.Ascending)
        .Option(o => o.Unique = true)
        .Create();

      // Customer Index
      DB.Index<Customer>()
        .Key(x => x.PhoneNo, KeyType.Descending)
        .Option(o => o.Unique = true)
        .Create();
      return services;
    }
  }
}