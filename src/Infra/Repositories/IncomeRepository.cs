using Domain.Interfaces;
using Domain.Entities;
using Infra.Context;
using Infra.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories;

public class IncomeRepository : Repository<Income>, IIncomeRepository
{
    public IncomeRepository(FinanceDbContext context) : base(context)
    { }

    public async Task<IEnumerable<Income>> GetAllIncomesAsync()
    {
        return await Entities
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Income> GetIncomeByIdAsync(Guid id)
    {
        return await Entities
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.Id == id)
            ?? throw new KeyNotFoundException($"Income with id {id} not found");
    }

    public async Task<(IEnumerable<Income> Items, int TotalCount)> GetPaginatedAsync(
        int pageNumber,
        int pageSize,
        string searchTerm = ""
    )
    {
        var query = Entities
            .AsNoTracking();

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(i =>
                i.Description.Contains(searchTerm) ||
                i.Category.ToString().Contains(searchTerm)
            );
        }
        
        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(i => i.Date)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }
}