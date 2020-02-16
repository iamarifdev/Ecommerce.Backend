using Ecommerce.Backend.Common.Configurations;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Entities;

namespace Ecommerce.Backend.API.Helpers
{
  public static class Extensions
  {
    // public static IWebHost MigrateDatabase<T>(this IWebHost webHost) where T : DbContext
    // {
    //   var serviceScopeFactory = (IServiceScopeFactory) webHost.Services.GetService(typeof(IServiceScopeFactory));
    //   using(var scope = serviceScopeFactory.CreateScope())
    //   {
    //     var services = scope.ServiceProvider;
    //     var dbContext = services.GetRequiredService<T>();
    //     dbContext.Database.Migrate();
    //   }
    //   return webHost;
    // }

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

    public static IServiceCollection RegisterAllIndex(this IServiceCollection services)
    {
      // Role Index
      // DB.Index<Role>()
      //   .Key(x => x.Name, KeyType.Ascending)
      //   .Option(o => o.Unique = true)
      //   .Create();
      return services;
    }
  }
}