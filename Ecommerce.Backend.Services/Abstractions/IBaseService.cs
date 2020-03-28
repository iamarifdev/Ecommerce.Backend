using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Ecommerce.Backend.Common.Models;
using Ecommerce.Backend.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Ecommerce.Backend.Services.Abstractions
{
  public interface IBaseService<TEntity> where TEntity : BaseEntityWithStatus
  {
    Task<PagedList<TEntity>> GetPaginatedList(PagedQuery query);
    Task<PagedList<TDto>> GetPaginatedList<TDto>(PagedQuery query, Expression<Func<TEntity, TDto>> projection) where TDto: class;
    Task<TEntity> GetByExpression(Expression<Func<TEntity, bool>> expression);
    Task<bool> IsExist(Expression<Func<TEntity, bool>> expression);
    Task<bool> IsExist(BsonDocument condition);
    Task<TEntity> GetById(string id);
    Task<TDto> GetById<TDto>(string id, Expression<Func<TEntity, TDto>> projection) where TDto: class;
    Task<TEntity> Add(TEntity entity);
    Task<TEntity> UpdateById(string id, TEntity entity);
    Task<TEntity> ToggleActivationById(string id, bool status);
    Task<TEntity> RemoveById(string id);
  }
}