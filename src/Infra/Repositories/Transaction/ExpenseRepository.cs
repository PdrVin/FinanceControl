using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Infra.Context;
using Infra.Repositories.Base;

namespace Infra.Repositories;

public class ExpenseRepository : Repository<Expense>, IExpenseRepository
{
    public ExpenseRepository(FinanceDbContext context) : base(context)
    { }

    public async Task<IEnumerable<Expense>> GetAllExpensesAsync()
    {
        return await Entities
            .Include(e => e.BankAccount)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Expense> GetExpenseByIdAsync(Guid id)
    {
        return await Entities
            .Include(e => e.BankAccount)
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.Id == id)
            ?? throw new KeyNotFoundException($"Expense with id {id} not found");
    }

    public async Task<IEnumerable<Expense>> GetExpensesByBankAccountIdAsync(Guid bankAccountId)
    {
        return await Entities
            .Where(e => e.BankAccountId == bankAccountId)
            .Include(e => e.BankAccount)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Expense>> GetExpensesByCategoryAsync(ExpenseCategory category)
    {
        return await Entities
            .Where(e => e.Category == category)
            .Include(e => e.BankAccount)
            .AsNoTracking()
            .ToListAsync();
    }  

    public async Task<(IEnumerable<Expense> Items, int TotalCount)> GetPaginatedAsync(
        int pageNumber,
        int pageSize,
        string searchTerm = ""
    )
    {
        var query = Entities
            .Include(e => e.BankAccount)
            .AsNoTracking();

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(e =>
                e.Description.Contains(searchTerm) ||
                e.Category.ToString().Contains(searchTerm) ||
                e.BankAccount!.ToString().Contains(searchTerm)
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