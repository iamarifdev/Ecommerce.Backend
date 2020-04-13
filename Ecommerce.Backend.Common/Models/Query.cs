namespace Ecommerce.Backend.Common.Models
{
  public class Query
  {
    public bool All { get; set; } = true;
    public string SearchTerm { get; set; } = null;

    /// <summary>
    /// `-1` for descending and `1` for ascending
    /// and it used in mongodb
    /// </summary>
    public int Order { get; set; } = -1;

    /// <summary>
    /// by default `updatedAt` will be used as sort field
    /// </summary>
    public string Sort { get; set; } = "updatedAt";
  }
}