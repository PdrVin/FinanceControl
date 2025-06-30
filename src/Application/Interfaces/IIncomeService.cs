using Application.DTOs;
using Application.Helpers.Pagination;
using Application.Interfaces.Base;
using Domain.Entities;

namespace Application.Interfaces;

public interface IIncomeService : IService<IncomeDto, Income>
{
    Task<IEnumerable<Income>> GetAllIncomeAsync();
    Task<Income?> GetIncomeByIdAsync(Guid id);

    Task AddIncomeAsync(IncomeDto income);
    Task UpdateIncomeAsync(IncomeDto income);

    Task<PagedResult<IncomeDto>> GetPaginatedIncomesAsync(
        int pageNumber, int pageSize, string searchTerm = "");
}
