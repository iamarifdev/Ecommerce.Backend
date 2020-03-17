using System.Threading.Tasks;
using Ecommerce.Backend.Common.Models;
using Ecommerce.Backend.Entities;

namespace Ecommerce.Backend.Services.Abstractions
{
  public interface IBaseService<TEntity> where TEntity : BaseEntityWithStatus
  {
    Task<PagedList<TEntity>> GetPaginatedList(PagedQuery query);
    Task<TEntity> GetById(string id);
    Task<TEntity> Add(TEntity entity);
    Task<TEntity> UpdateById(string id, TEntity entity);
    Task<TEntity> ToggleActivationById(string id, bool status);
    Task<TEntity> RemoveById(string id);
  }
}
