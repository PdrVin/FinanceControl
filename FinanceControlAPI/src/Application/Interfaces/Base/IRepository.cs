using Domain.Entities.Base;

namespace Application.Interfaces.Base;

public interface IRepository<T> where T : IEntity
{
    Task<List<T>> GetAllAsync(int skip, int take);
    Task<T> GetByIdAsync(Guid id);
    Task<T> CreateAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task DeleteAsync(Guid id);
}
