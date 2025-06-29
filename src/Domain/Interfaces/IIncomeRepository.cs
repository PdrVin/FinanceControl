using Domain.Interfaces.Base;
using Domain.Entities;

namespace Domain.Interfaces;

public interface IIncomeRepository : IRepository<Income>
{
    Task<IEnumerable<Income>> GetAllIncomesAsync();
    Task<Income> GetIncomeByIdAsync(Guid id);
    Task<(IEnumerable<Income> Items, int TotalCount)> GetPaginatedAsync(
        int pageNumber, int pageSize, string searchTerm = "");
}