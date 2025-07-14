using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Infra.Context;
using Infra.Repositories.Base;

namespace Infra.Repositories;

public class CardExpenseRepository : Repository<CardExpense>, ICardExpenseRepository
{
    public CardExpenseRepository(FinanceDbContext context) : base(context) { }

    public async Task<IEnumerable<CardExpense>> GetExpensesByCreditCardIdAsync(Guid creditCardId)
    {
        return await Entities
            .Where(ce => ce.CreditCardId == creditCardId)
            .Include(ce => ce.CreditCard)
            .Include(ce => ce.Invoice)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<CardExpense>> GetExpensesByInvoiceIdAsync(Guid invoiceId)
    {
        return await Entities
            .Where(ce => ce.InvoiceId == invoiceId)
            .Include(ce => ce.CreditCard)
            .Include(ce => ce.Invoice)
            .AsNoTracking()
            .ToListAsync();
    }
}