using Application.Interfaces.Base;
using Domain.Entities.Base;
using Domain.Interfaces.Base;
using AutoMapper;

namespace Application.Services.Base;

public class Service<TRequest, TResponse, TEntity>
    : IService<TRequest, TResponse, TEntity>
        where TEntity : class, IEntity
        where TRequest : class
        where TResponse : class
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

    public async Task<IEnumerable<TResponse>> GetAllAsync()
    {
        var entities = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<TResponse>>(entities);
    }

    public async Task<TResponse> GetByIdAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return _mapper.Map<TResponse>(entity);
    }

    public async Task AddAsync(TRequest request)
    {
        var entity = _mapper.Map<TEntity>(request);

        await _repository.SaveAsync(entity);
        await _unitOfWork.CommitAsync();
    }

    public async Task UpdateAsync(Guid id, TRequest request)
    {
        var existingEntity = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Entity with id {id} not found.");

        _mapper.Map(request, existingEntity);

        _repository.Update(existingEntity);
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
