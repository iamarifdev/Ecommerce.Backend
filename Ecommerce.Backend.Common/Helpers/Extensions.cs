using System.Collections.Generic;
using Ecommerce.Backend.Common.Models;

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
  }
}