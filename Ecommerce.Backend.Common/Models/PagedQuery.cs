namespace Ecommerce.Backend.Common.Models
{
  public class PagedQuery : Query
  {
    public int PageSize { get; set; } = 20;
    public int Page { get; set; } = 1;
  }
}