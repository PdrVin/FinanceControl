using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Base;

namespace Domain.Interfaces;

public interface IExpenseRepository : IRepository<Expense>
{
    Task<IEnumerable<Expense>> GetAllExpensesAsync();
    Task<Expense> GetExpenseByIdAsync(Guid id);
    Task<IEnumerable<Expense>> GetExpensesByBankAccountIdAsync(Guid bankAccountId);
    Task<IEnumerable<Expense>> GetExpensesByCategoryAsync(ExpenseCategory category);
    Task<IEnumerable<Expense>> GetExpensesByPeriodAsync(DateTime startDate, DateTime endDate);

    Task<(IEnumerable<Expense> Items, int TotalCount)> GetPaginatedAsync(
        int pageNumber, int pageSize, string searchTerm = "");
}

