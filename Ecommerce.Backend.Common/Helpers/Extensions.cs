using System.Collections.Generic;
using System.Text.Json;
using Ecommerce.Backend.Common.Models;

namespace Ecommerce.Backend.Common.Helpers
{
  public static class Extensions
  {
    public static bool IsEmpty(this object obj) => obj == null;
    public static bool IsEmpty(this string str) => string.IsNullOrWhiteSpace(str);
    public static bool IsNotEmpty(this object obj) => obj != null;
    public static bool IsNotEmpty(this string str) => !string.IsNullOrWhiteSpace(str);
    public static PagedList<T> ToPagedList<T>(this List<T> items, int totalCount) where T : class
    {
      return new PagedList<T>
      {
        Count = totalCount,
        Items = items
      };
    }

    public static string ToJson<T>(this IEnumerable<T> items)
    {
      var options = new JsonSerializerOptions
      {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
      };
      var json = JsonSerializer.Serialize(items, options);
      return json;
    }
  }
}