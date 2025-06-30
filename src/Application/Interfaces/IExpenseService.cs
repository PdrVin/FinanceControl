using Application.DTOs;
using Application.Helpers.Pagination;
using Application.Interfaces.Base;
using Domain.Entities;

namespace Application.Interfaces;

public interface IExpenseService : IService<ExpenseDto, Expense>
{
    Task<IEnumerable<Expense>> GetAllExpensesAsync();
    Task<Expense?> GetExpenseByIdAsync(Guid id);

    Task AddExpenseAsync(ExpenseDto expense);
    Task UpdateExpenseAsync(ExpenseDto expense);

    Task<PagedResult<ExpenseDto>> GetPaginatedExpensesAsync(
        int pageNumber, int pageSize, string searchTerm = "");
}
