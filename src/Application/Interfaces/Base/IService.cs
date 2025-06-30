using Domain.Entities.Base;

namespace Application.Interfaces.Base;

public interface IService<TDto, TEntity>
    where TEntity : IEntity
{
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<TEntity> GetByIdAsync(Guid id);
    Task AddAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(Guid id);
    Task<int> CountAsync();
}
