using Domain.Entities;

namespace Application.Interfaces;

public interface IExpenseRepository
{
    Task<IEnumerable<Expense>> GetAllExpenses();
    Task<Expense?> GetExpenseById(int id);
    Task AddExpense(Expense expense);
    Task UpdateExpense(Expense expense);
    Task DeleteExpense(int id);
}

