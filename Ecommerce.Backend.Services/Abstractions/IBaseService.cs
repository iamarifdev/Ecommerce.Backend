using System;
using System.Collections.Generic;
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
    Task<List<TEntity>> GetList(Query query);
    Task<PagedList<TEntity>> GetPaginatedList(PagedQuery query);
    Task<PagedList<TDto>> GetPaginatedList<TDto>(PagedQuery query, Expression<Func<TEntity, TDto>> projection) where TDto: class;
    Task<TEntity> GetByExpression(Expression<Func<TEntity, bool>> expression);
    Task<IEnumerable<TEntity>> GetAllByExpression(Expression<Func<TEntity, bool>> expression);
    Task<bool> IsExist(Expression<Func<TEntity, bool>> expression);
    Task<bool> IsExist(BsonDocument condition);
    Task<TEntity> GetById(string id);
    Task<TDto> GetById<TDto>(string id, Expression<Func<TEntity, TDto>> projection) where TDto: class;
    Task<bool> ValidateIdentity(Dictionary<string, string> keyValues);
    Task<IEnumerable<TEntity>> AddRange(IEnumerable<TEntity> entities);
    Task<TEntity> Add(TEntity entity);
    Task<TEntity> UpdatePartial(Expression<Func<TEntity, bool>> condition, UpdateDefinition<TEntity> update);
    Task<TEntity> UpdateById(string id, TEntity entity);
    Task<TEntity> ToggleActivationById(string id, bool status);
    Task<TEntity> RemoveById(string id);
    Task<bool> DeleteById(string id);
    Task<bool> DeleteByExpression(Expression<Func<TEntity, bool>> expression);
  }
}