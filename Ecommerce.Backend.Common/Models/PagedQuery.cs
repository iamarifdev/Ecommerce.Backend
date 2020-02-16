namespace Ecommerce.Backend.Common.Models
{
  public class PagedQuery
  {
    public int PageSize { get; set; } = 20;
    public int Page { get; set; } = 1;
    public bool All { get; set; } = true;
    public string SearchTerm { get; set; } = null;
    public string Order { get; set; } = null;
    public string Sort { get; set; } = null;
  }
}