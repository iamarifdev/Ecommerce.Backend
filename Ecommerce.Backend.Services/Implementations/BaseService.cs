using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Ecommerce.Backend.Common.Helpers;
using Ecommerce.Backend.Common.Models;
using Ecommerce.Backend.Entities;
using Ecommerce.Backend.Services.Abstractions;
using MongoDB.Driver;
using MongoDB.Entities;

namespace Ecommerce.Backend.Services.Implementations
{
  public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : BaseEntityWithStatus
  {
    private readonly IMongoCollection<TEntity> _entities;
    private readonly FindOneAndUpdateOptions<TEntity, TEntity> _options = new FindOneAndUpdateOptions<TEntity, TEntity> { ReturnDocument = ReturnDocument.After };
    public BaseService()
    {
      _entities = DB.Collection<TEntity>();
    }

    public async Task<TEntity> Add(TEntity entity)
    {
      await _entities.InsertOneAsync(entity);
      return entity;
    }

    public async Task<TEntity> GetById(string id)
    {
      var entity = await _entities.FindAsync<TEntity>(x => x.ID == id).Result.FirstOrDefaultAsync();
      return entity;
    }

    public async Task<PagedList<TEntity>> GetPaginatedList(PagedQuery query)
    {
      Expression<Func<TEntity, bool>> allConditions = (entity) => !entity.IsDeleted;
      Expression<Func<TEntity, bool>> conditions = (entity) => !entity.IsDeleted && entity.IsEnabled;

      var filterConditions = Builders<TEntity>.Filter.Where(query.All ? allConditions : conditions);

      var count = (int) await _entities.CountDocumentsAsync(filterConditions);
      var entities = await _entities
        .Find(filterConditions)
        .SortBy(product => product.UpdatedAt)
        .Skip(query.PageSize * (query.Page - 1))
        .Limit(query.PageSize)
        .ToListAsync();
      return entities.ToPagedList(count);
    }

    public async Task<TEntity> RemoveById(string id)
    {
      var update = Builders<TEntity>.Update
        .Set(s => s.IsEnabled, false)
        .Set(s => s.IsDeleted, true)
        .Set(s => s.UpdatedAt, DateTime.Now);
      var deletedEntity = await _entities.FindOneAndUpdateAsync<TEntity>(x => x.ID == id, update, _options);
      return deletedEntity;
    }

    public async Task<TEntity> ToggleActivationById(string id, bool status)
    {
      var update = Builders<TEntity>.Update
        .Set(s => s.IsEnabled, status)
        .Set(s => s.UpdatedAt, DateTime.Now);
      var updatedEntity = await _entities.FindOneAndUpdateAsync<TEntity>(x => x.ID == id, update, _options);
      return updatedEntity;
    }

    public async Task<TEntity> UpdateById(string id, TEntity entity)
    {
      var filterDef = Builders<TEntity>.Filter;
      var updateDef = Builders<TEntity>.Update;

      var condition = filterDef.Eq(x => x.ID, id);

      Type type = typeof(TEntity);
      var properties = type.GetProperties();

      var update = updateDef.Set(s => s.UpdatedAt, DateTime.Now);
      foreach (var property in properties)
      {
        if (property.Name == "ID") continue;
        if (property.Name == "CreatedAt") continue;
        var value = property.GetValue(entity);
        if (value == null) continue;
        update = update.Set(property.Name, value);
      }
      var updatedEntity = await _entities.FindOneAndUpdateAsync(condition, update, _options);
      return updatedEntity;
    }
  }
}
