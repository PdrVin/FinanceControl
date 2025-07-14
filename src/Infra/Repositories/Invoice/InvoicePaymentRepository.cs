using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Interfaces;
using Infra.Context;
using Infra.Repositories.Base;

namespace Infra.Repositories;

public class InvoicePaymentRepository : Repository<InvoicePayment>, IInvoicePaymentRepository
{
    public InvoicePaymentRepository(FinanceDbContext context) : base(context) { }

    public async Task<IEnumerable<InvoicePayment>> GetPaymentsByInvoiceIdAsync(Guid invoiceId)
    {
        return await Entities
            .Where(ip => ip.InvoiceId == invoiceId)
            .OrderBy(ip => ip.PaymentDate)
            .AsNoTracking() 
            .ToListAsync();
    }

    public async Task<IEnumerable<InvoicePayment>> GetPaymentsByBankAccountIdAsync(Guid bankAccountId)
    {
        return await Entities
            .Where(ip => ip.BankAccountId == bankAccountId)
            .OrderBy(ip => ip.PaymentDate)
            .AsNoTracking()
            .ToListAsync();
    }
}