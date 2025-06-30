using Application.DTOs;
using Application.Helpers.Pagination;
using Application.Interfaces.Base;
using Domain.Entities;

namespace Application.Interfaces;

public interface IInvoiceService : IService<InvoiceDto, Invoice>
{
    Task<IEnumerable<Invoice>> GetAllInvoiceAsync();
    Task<Invoice?> GetInvoiceByIdAsync(Guid id);

    Task AddInvoiceAsync(InvoiceDto invoice);
    Task UpdateInvoiceAsync(InvoiceDto invoice);

    Task<PagedResult<InvoiceDto>> GetPaginatedInvoicesAsync(
        int pageNumber, int pageSize, string searchTerm = "");
}
