using System.Collections.Generic;
using Ecommerce.Backend.Common.Configurations;
using Ecommerce.Backend.Common.Models;
using Ecommerce.Backend.Entities;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Entities;

namespace Ecommerce.Backend.Common.Helpers
{
  public static class Extensions
  {
    public static bool IsEmpty(this object obj) => obj == null;
    public static bool IsEmpty(this string str) => str == null || str.IsEmpty();
    public static bool IsNotEmpty(this object obj) => obj != null;
    public static bool IsNotEmpty(this string str) => str != null || !str.IsEmpty();
    public static PagedList<T> ToPagedList<T>(this List<T> items, int totalCount) where T : class
    {
      return new PagedList<T>
      {
        Count = totalCount,
        Items = items
      };
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
      DB.Migrate<BaseEntity>();
      return services;
    }

    public static IServiceCollection RegisterAllIndex(this IServiceCollection services)
    {
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