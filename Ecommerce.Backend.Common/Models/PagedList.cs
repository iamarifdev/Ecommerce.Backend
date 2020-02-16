using System.Collections.Generic;

namespace Ecommerce.Backend.Common.Models
{
  public class PagedList<T>
  {
    public int Count { get; set; }
    public List<T> Items { get; set; } = new List<T>();
  }
}