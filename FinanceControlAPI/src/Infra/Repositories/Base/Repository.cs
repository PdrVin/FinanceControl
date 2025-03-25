using Application.Interfaces.Base;
using Domain.Entities.Base;
using Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories.Base;
public class Repository<T>
    : IRepository<T> where T : class, IEntity
{
    private readonly FinanceDbContext _context;
    public DbSet<T> Entities { get; }

    public Repository(FinanceDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        Entities = _context.Set<T>();
    }

    public async Task<List<T>> GetAllAsync(int skip = 0, int take = 25)
    {
        return await Entities.AsNoTracking().Skip(skip).Take(take).ToListAsync();
    }

    public async Task<T> GetByIdAsync(Guid id)
    {
        return await Entities.FindAsync(id) ??
            throw new KeyNotFoundException($"Entity with id {id} not found");
    }

    public async Task<T> CreateAsync(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        await Entities.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<T> UpdateAsync(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await GetByIdAsync(id);
        ArgumentNullException.ThrowIfNull(entity);
        Entities.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context?.Dispose();
    }
}