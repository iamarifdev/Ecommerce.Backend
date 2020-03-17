using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Ecommerce.Backend.Common.DTO;
using Ecommerce.Backend.Common.Helpers;
using Ecommerce.Backend.Common.Models;
using Ecommerce.Backend.Entities;
using Ecommerce.Backend.Services.Abstractions;
using MongoDB.Driver;
using MongoDB.Entities;

namespace Ecommerce.Backend.Services.Implementations
{
  public class ProductService : IProductService
  {
    private readonly IMongoCollection<Product> _products;
    private readonly FindOneAndUpdateOptions<Product, Product> _options = new FindOneAndUpdateOptions<Product, Product> { ReturnDocument = ReturnDocument.After };

    public ProductService()
    {
      _products = DB.Collection<Product>();
    }

    public async Task<PagedList<ProductListItemDto>> GetPaginatedProducts(PagedQuery query)
    {
      Expression<Func<Product, bool>> allConditions = (product) => !product.IsDeleted;
      Expression<Func<Product, bool>> conditions = (product) => !product.IsDeleted && product.IsEnabled;

      var filterConditions = Builders<Product>.Filter.Where(query.All ? allConditions : conditions);

      var count = (int) await _products.CountDocumentsAsync(filterConditions);
      var products = await _products
        .Find(filterConditions)
        .Project(product => new ProductListItemDto(
          product.ID,
          product.SKU,
          product.Title,
          product.Description,
          product.FeatureImageUrl,
          new ManufactureDetailDto(product.ManufactureDetail.ModelNo, product.ManufactureDetail.ReleaseDate),
          new PricingDto { Price = product.Pricing.Price },
          product.ProductColors.Select(s => new ProductColorDto(s.ColorCode, s.ColorName)),
          product.IsEnabled,
          product.CreatedAt,
          product.UpdatedAt
        ))
        .SortBy(product => product.Title)
        .Skip(query.PageSize * (query.Page - 1))
        .Limit(query.PageSize)
        .ToListAsync();
      return products.ToPagedList(count);
    }

    public async Task<Product> GetProductById(string productId)
    {
      var product = await _products.FindAsync<Product>(x => x.ID == productId).Result.FirstOrDefaultAsync();
      return product;
    }

    public async Task<Product> AddProduct(Product product)
    {
      product.ProductColors.ForEach(productColor =>
      {
        productColor.ColorCode = productColor.ColorCode.ToLower();
        if (!productColor.ColorCode.Contains("#"))
        {
          productColor.ColorCode = $"#{productColor.ColorCode}";
        }
      });
      await _products.InsertOneAsync(product);
      return product;
    }

    public async Task<Product> UpdateProductById(string productId, Product product)
    {
      var filterDef = Builders<Product>.Filter;
      var updateDef = Builders<Product>.Update;

      var condition = filterDef.Eq(x => x.ID, productId);
      var update = updateDef
        .Set(s => s.Title, product.Title)
        .Set(s => s.SKU, product.SKU)
        .Set(s => s.Description, product.Description)
        .Set("ManufactureDetail", product.ManufactureDetail)
        .Set("ProductColors", product.ProductColors)
        .Set(s => s.Pricing.Price, product.Pricing.Price)
        .Set(s => s.FeatureImageUrl, product.FeatureImageUrl)
        .Set(s => s.IsEnabled, product.IsEnabled)
        .Set(s => s.UpdatedAt, DateTime.Now);
      var updatedProduct = await _products.FindOneAndUpdateAsync(condition, update, _options);
      return updatedProduct;
    }

    public async Task<Product> UpdateFeatureImage(string productId, string featureImageUrl)
    {
      var update = Builders<Product>.Update.Set("FeatureImageUrl", featureImageUrl);
      var updatedProduct = await _products.FindOneAndUpdateAsync<Product, Product>(r => r.ID == productId, update, _options);
      return updatedProduct;
    }

    public async Task<Product> UpdateImages(string productId, string color, IEnumerable<string> imageUrls)
    {
      var filter = Builders<Product>.Filter;
      var condition = filter.Eq(x => x.ID, productId) & filter.Eq("productColors.colorCode", $"#{color.ToLower()}");
      var update = Builders<Product>.Update.AddToSetEach("productColors.$.images", imageUrls);
      var result = await _products.UpdateOneAsync(condition, update);
      var updatedProduct = await GetProductById(productId);
      return updatedProduct;
    }

    public async Task<Product> ToggleActivationById(string productId, bool status)
    {
      var update = Builders<Product>.Update
        .Set(s => s.IsEnabled, status)
        .Set(s => s.UpdatedAt, DateTime.Now);
      var updatedProduct = await _products.FindOneAndUpdateAsync<Product>(x => x.ID == productId, update, _options);
      return updatedProduct;
    }

    public async Task<Product> RemoveProductById(string productId)
    {
      var update = Builders<Product>.Update
        .Set(s => s.IsEnabled, false)
        .Set(s => s.IsDeleted, true)
        .Set(s => s.UpdatedAt, DateTime.Now);
      var deletedProduct = await _products.FindOneAndUpdateAsync<Product>(x => x.ID == productId, update, _options);
      return deletedProduct;
    }

  }
}