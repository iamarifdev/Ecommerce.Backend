using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Ecommerce.Backend.Common.Helpers;
using Ecommerce.Backend.Common.Models;
using Ecommerce.Backend.Entities;
using MongoDB.Driver;
using MongoDB.Entities;

namespace Ecommerce.Backend.Services.Abstractions
{
    public interface IProductService
    {
        Task<PagedList<Product>> GetPaginatedProducts(PagedQuery query);
        Task<Product> AddProduct(Product product);

    }
}