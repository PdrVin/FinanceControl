using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Infra.Context;
using Infra.Repositories.Base;

namespace Infra.Repositories;

public class IncomeRepository : Repository<Income>, IIncomeRepository
{
    public IncomeRepository(FinanceDbContext context) : base(context)
    { }

    public async Task<IEnumerable<Income>> GetAllIncomesAsync()
    {
        return await Entities
            .Include(i => i.BankAccount)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Income> GetIncomeByIdAsync(Guid id)
    {
        return await Entities
            .Include(i => i.BankAccount)
            .FirstOrDefaultAsync(i => i.Id == id)
            ?? throw new KeyNotFoundException($"Income with id {id} not found");
    }

    public async Task<IEnumerable<Income>> GetIncomesByBankAccountIdAsync(Guid bankAccountId)
    {
        return await Entities
            .Where(i => i.BankAccountId == bankAccountId)
            .Include(i => i.BankAccount)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Income>> GetIncomesByCategoryAsync(IncomeCategory category)
    {
        return await Entities
            .Where(i => i.Category == category)
            .Include(i => i.BankAccount)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Income>> GetIncomesByPeriodAsync(DateTime startDate, DateTime endDate)
    {
        return await Entities
            .Where(i => i.Date >= startDate && i.Date <= endDate)
            .Include(i => i.BankAccount)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<(IEnumerable<Income> Items, int TotalCount)> GetPaginatedAsync(
        int pageNumber,
        int pageSize,
        string searchTerm = ""
    )
    {
        var query = Entities
            .Include(i => i.BankAccount)
            .AsNoTracking();

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(i =>
                i.Description.Contains(searchTerm) ||
                i.Category.ToString().Contains(searchTerm) ||
                i.BankAccount!.ToString().Contains(searchTerm)
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