using Domain.Interfaces.Base;
using Domain.Entities;

namespace Domain.Interfaces;

public interface IInvoiceRepository : IRepository<Invoice>
{
    Task<IEnumerable<Invoice>> GetAllInvoicesAsync();
    Task<Invoice> GetInvoiceByIdAsync(Guid id);
    Task<(IEnumerable<Invoice> Items, int TotalCount)> GetPaginatedAsync(
        int pageNumber, int pageSize, string searchTerm = "");
}