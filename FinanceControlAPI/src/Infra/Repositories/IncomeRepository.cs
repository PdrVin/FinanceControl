using Application.Interfaces;
using Domain.Entities;
using Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories;

public class IncomeRepository(FinanceDbContext context) : IIncomeRepository
{
    private readonly FinanceDbContext _context = context;

    public async Task<IEnumerable<Income>> GetAllIncomes()
    {
        return await _context.Incomes.ToListAsync();
    }

    public async Task<Income?> GetIncomeById(Guid id)
    {
        return await _context.Incomes.FindAsync(id);
    }

    public async Task AddIncome(Income income)
    {
        _context.Incomes.Add(income);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateIncome(Income income)
    {
        _context.Incomes.Update(income);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteIncome(Guid id)
    {
        var income = await _context.Incomes.FindAsync(id);
        if (income != null)
        {
            _context.Incomes.Remove(income);
            await _context.SaveChangesAsync();
        }
    }
}