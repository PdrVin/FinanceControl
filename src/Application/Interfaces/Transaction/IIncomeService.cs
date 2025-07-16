using Application.DTOs.Incomes;
using Application.Helpers;
using Application.Interfaces.Base;
using Domain.Entities;
using Domain.Enums;

namespace Application.Interfaces;

public interface IIncomeService : IService<IncomeRequest, IncomeResponse, Income>
{
    Task<IEnumerable<IncomeResponse>> GetAllIncomesAsync();
    Task<IncomeResponse> GetIncomeByIdAsync(Guid id);
    Task<IEnumerable<IncomeResponse>> GetIncomesByBankAccountIdAsync(Guid bankAccountId);
    Task<IEnumerable<IncomeResponse>> GetIncomesByCategoryAsync(IncomeCategory category);

    Task CreateIncomeAsync(IncomeRequest request);
    Task UpdateIncomeAsync(Guid id, IncomeRequest request);
    Task DeleteIncomeAsync(Guid id);

    Task<PagedResult<IncomeResponse>> GetPaginatedIncomesAsync(
        int pageNumber, int pageSize, string searchTerm = "");
}