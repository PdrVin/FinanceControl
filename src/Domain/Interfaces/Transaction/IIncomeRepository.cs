using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Base;

namespace Domain.Interfaces;

public interface IIncomeRepository : IRepository<Income>
{
    Task<IEnumerable<Income>> GetAllIncomesAsync();
    Task<Income> GetIncomeByIdAsync(Guid id);
    Task<IEnumerable<Income>> GetIncomesByBankAccountIdAsync(Guid bankAccountId);
    Task<IEnumerable<Income>> GetIncomesByCategoryAsync(IncomeCategory category);
    Task<(IEnumerable<Income> Items, int TotalCount)> GetPaginatedAsync(
        int pageNumber, int pageSize, string searchTerm = "");
}