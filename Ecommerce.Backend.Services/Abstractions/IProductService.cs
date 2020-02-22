using System.Collections.Generic;
using System.Threading.Tasks;
using Ecommerce.Backend.Common.Models;
using Ecommerce.Backend.Entities;

namespace Ecommerce.Backend.Services.Abstractions
{
  public interface IProductService
  {
    Task<PagedList<Product>> GetPaginatedProducts(PagedQuery query);
    Task<Product> GetProductById(string productId);
    Task<Product> AddProduct(Product product);
    Task<Product> UpdateFeatureImage(string productId, string featureImageUrl);
    Task<Product> UpdateImages(string productId, List<string> imageUrls);

  }
}