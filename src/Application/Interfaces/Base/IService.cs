using Domain.Entities.Base;

namespace Application.Interfaces.Base;

public interface IService<TRequest, TResponse, TEntity>
    where TEntity : IEntity
    where TRequest : class
    where TResponse : class
{
    Task<IEnumerable<TResponse>> GetAllAsync();
    Task<TResponse> GetByIdAsync(Guid id);
    Task AddAsync(TRequest request);
    Task UpdateAsync(Guid id, TRequest request);
    Task DeleteAsync(Guid id);
    Task<int> CountAsync();
}
