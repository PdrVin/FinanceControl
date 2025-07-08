using Domain.Interfaces.Base;
using Domain.Entities;

namespace Domain.Interfaces;

public interface IExpenseRepository : IRepository<Expense>
{
    Task<IEnumerable<Expense>> GetAllExpensesAsync();
    Task<Expense> GetExpenseByIdAsync(Guid id);
    Task<(IEnumerable<Expense> Items, int TotalCount)> GetPaginatedAsync(
        int pageNumber, int pageSize, string searchTerm = "");
    Task<IEnumerable<Expense>> GetExpensesByPeriodAsync(
        int? year = null, int? month = null, string searchTerm = "");
}

