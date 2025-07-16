using Application.DTOs.Expenses;
using Application.Helpers;
using Application.Interfaces.Base;
using Domain.Entities;
using Domain.Enums;

namespace Application.Interfaces;

public interface IExpenseService : IService<ExpenseRequest, ExpenseResponse, Expense>
{
    Task<IEnumerable<ExpenseResponse>> GetAllExpensesAsync();
    Task<ExpenseResponse> GetExpenseByIdAsync(Guid id);
    Task<IEnumerable<ExpenseResponse>> GetExpensesByBankAccountIdAsync(Guid bankAccountId);
    Task<IEnumerable<ExpenseResponse>> GetExpensesByCategoryAsync(ExpenseCategory category);

    Task CreateExpenseAsync(ExpenseRequest request);
    Task UpdateExpenseAsync(Guid id, ExpenseRequest request);
    Task DeleteExpenseAsync(Guid id);

    Task<PagedResult<ExpenseResponse>> GetPaginatedExpensesAsync(
        int pageNumber, int pageSize, string searchTerm = "");
}