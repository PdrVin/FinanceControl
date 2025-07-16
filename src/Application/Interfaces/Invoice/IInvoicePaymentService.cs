using Application.DTOs.InvoicePayments;
using Application.Interfaces.Base;
using Domain.Entities;

namespace Application.Interfaces;

public interface IInvoicePaymentService : IService<InvoicePaymentRequest, InvoicePaymentResponse, InvoicePayment>
{
    Task<IEnumerable<InvoicePaymentResponse>> GetPaymentsByInvoiceIdAsync(Guid invoiceId);
    Task<IEnumerable<InvoicePaymentResponse>> GetPaymentsByBankAccountIdAsync(Guid bankAccountId);
    Task<InvoicePaymentResponse> GetInvoicePaymentByIdAsync(Guid id);

    Task CreateInvoicePaymentAsync(InvoicePaymentRequest request);
    Task UpdateInvoicePaymentAsync(Guid id, InvoicePaymentRequest request);
    Task DeleteInvoicePaymentAsync(Guid id);
}