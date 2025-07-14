using Domain.Entities;
using Domain.Interfaces.Base;

namespace Domain.Interfaces;

public interface IInvoicePaymentRepository : IRepository<InvoicePayment>
{
    Task<IEnumerable<InvoicePayment>> GetPaymentsByInvoiceIdAsync(Guid invoiceId);
    Task<IEnumerable<InvoicePayment>> GetPaymentsByBankAccountIdAsync(Guid bankAccountId);
}