using Domain.Entities;
using Domain.Interfaces.Base;

namespace Domain.Interfaces;

public interface IInvoiceRepository : IRepository<Invoice>
{
    Task<IEnumerable<Invoice>> GetAllInvoicesAsync();
    Task<Invoice> GetInvoiceByIdAsync(Guid id);
    Task<IEnumerable<Invoice>> GetInvoicesByCreditCardIdAsync(Guid creditCardId);
    Task<Invoice?> GetInvoiceByPeriodAsync(int month, int year);
    Task<(IEnumerable<Invoice> Items, int TotalCount)> GetPaginatedAsync(
        int pageNumber, int pageSize, string searchTerm = "");
}