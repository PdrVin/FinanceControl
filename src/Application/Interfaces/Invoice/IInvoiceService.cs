using Application.DTOs.Invoices;
using Application.Helpers;
using Application.Interfaces.Base;
using Domain.Entities;

namespace Application.Interfaces;

public interface IInvoiceService : IService<InvoiceRequest, InvoiceResponse, Invoice>
{
    Task<IEnumerable<InvoiceResponse>> GetAllInvoicesAsync();
    Task<InvoiceResponse> GetInvoiceByIdAsync(Guid id);

    Task<IEnumerable<InvoiceResponse>> GetInvoicesByCreditCardIdAsync(Guid creditCardId);
    Task<InvoiceResponse?> GetInvoiceByPeriodAsync(int month, int year);

    Task CreateInvoiceAsync(InvoiceRequest invoice);
    Task UpdateInvoiceAsync(Guid id, InvoiceRequest invoice);

    Task<PagedResult<InvoiceResponse>> GetPaginatedInvoicesAsync(
        int pageNumber, int pageSize, string searchTerm = "");
}
