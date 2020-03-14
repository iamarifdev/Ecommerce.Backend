using System.Collections.Generic;
using System.Threading.Tasks;
using Ecommerce.Backend.Common.DTO;
using Ecommerce.Backend.Common.Models;
using Ecommerce.Backend.Entities;

namespace Ecommerce.Backend.Services.Abstractions
{
  public interface IProductService
  {
    Task<PagedList<ProductListItemDto>> GetPaginatedProducts(PagedQuery query);
    Task<Product> GetProductById(string productId);
    Task<Product> AddProduct(Product product);
    Task<Product> UpdateProductById(string productId, Product product);
    Task<Product> ToggleActivationById(string productId, bool status);
    Task<Product> RemoveProductById(string productId);
    Task<Product> UpdateFeatureImage(string productId, string featureImageUrl);
    Task<Product> UpdateImages(string productId, string color, IEnumerable<string> imageUrls);

  }
}