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

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        try
        {
            return await Entities.AsNoTracking().ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error fetching all entities", ex);
        }
    }

    public async Task<T> GetByIdAsync(Guid id)
    {
        try
        {
            return await Entities.FindAsync(id) ??
                throw new KeyNotFoundException($"Entity with id {id} not found");
        }
        catch (Exception ex)
        {
            throw new Exception($"Error fetching entity with id {id}", ex);
        }
    }

    public async Task SaveAsync(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        try
        {
            await Entities.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error saving entity", ex);
        }
    }

    public async Task SaveRangeAsync(IEnumerable<T> entities)
    {
        ArgumentNullException.ThrowIfNull(entities);

        try
        {
            await Entities.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error saving entities", ex);
        }
    }

    public void Update(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        try
        {
            Entities.Update(entity);
            _context.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception("Error updating entity", ex);
        }
    }

    public void Delete(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        try
        {
            Entities.Remove(entity);
            _context.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception("Error deleting entity", ex);
        }
    }

    public void Dispose()
    {
        _context?.Dispose();
    }
}