using Domain.Interfaces;
using Domain.Entities;
using Infra.Context;
using Infra.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories;

public class ExpenseRepository : Repository<Expense>, IExpenseRepository
{
    public ExpenseRepository(FinanceDbContext context) : base(context)
    { }

    public async Task<IEnumerable<Expense>> GetAllExpensesAsync()
    {
        return await Entities
            .Include(e => e.Invoice)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Expense> GetExpenseByIdAsync(Guid id)
    {
        return await Entities
            .Include(e => e.Invoice)
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.Id == id)
            ?? throw new KeyNotFoundException($"Expense with id {id} not found");
    }

    public async Task<(IEnumerable<Expense> Items, int TotalCount)> GetPaginatedAsync(
        int pageNumber,
        int pageSize,
        string searchTerm = ""
    )
    {
        var query = Entities
            .Include(e => e.Invoice)
            .AsNoTracking();

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(e =>
                e.Description.Contains(searchTerm) ||
                e.Category.ToString().Contains(searchTerm) ||
                e.Account.ToString().Contains(searchTerm)
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

    public async Task<IEnumerable<Expense>> GetExpensesByPeriodAsync(
        int? year = null,
        int? month = null,
        string searchTerm = ""
    )
    {
        var query = Entities
            .Include(e => e.Invoice)
            .AsNoTracking();

        if (year.HasValue)
        {
            query = query.Where(e => e.Date.Year == year.Value);
        }

        if (month.HasValue)
        {
            query = query.Where(e => e.Date.Month == month.Value);
        }

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(e =>
                e.Description.Contains(searchTerm) ||
                e.Category.ToString().Contains(searchTerm) ||
                e.Account.ToString().Contains(searchTerm)
            );
        }

        return await query
            .OrderByDescending(e => e.Date)
            .ToListAsync();
    }
}