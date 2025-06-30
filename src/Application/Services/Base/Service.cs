using Application.Interfaces.Base;
using Domain.Entities.Base;
using Domain.Interfaces.Base;
using AutoMapper; 

namespace Application.Services.Base;

public class Service<TDto, TEntity> : IService<TDto, TEntity>
        where TEntity : class, IEntity
{
    private readonly IRepository<TEntity> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public Service(
        IRepository<TEntity> repository,
        IUnitOfWork unitOfWork,
        IMapper mapper
    )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<TEntity> GetByIdAsync(Guid id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task AddAsync(TEntity entity)
    {
        await _repository.SaveAsync(entity);
        await _unitOfWork.CommitAsync();
    }

    public async Task UpdateAsync(TEntity entity)
    {
        _repository.Update(entity);
        await _unitOfWork.CommitAsync();
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(Guid id)
    {
        _repository.Delete(id);
        await _unitOfWork.CommitAsync();
        await Task.CompletedTask;
    }

    public async Task<int> CountAsync()
    {
        return await _repository.CountAsync();
    }
}
