using Application.Interfaces;
using Domain.Entities;
using Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories;

public class ExpenseRepository(FinanceDbContext context) : IExpenseRepository
{
    private readonly FinanceDbContext _context = context;

    public async Task<IEnumerable<Expense>> GetAllExpenses()
    {
        return await _context.Expenses.ToListAsync();
    }

    public async Task<Expense?> GetExpenseById(Guid id)
    {
        return await _context.Expenses.FindAsync(id);
    }

    public async Task AddExpense(Expense expense)
    {
        _context.Expenses.Add(expense);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateExpense(Expense expense)
    {
        _context.Expenses.Update(expense);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteExpense(Guid id)
    {
        var expense = await _context.Expenses.FindAsync(id);
        if (expense != null)
        {
            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();
        }
    }
}