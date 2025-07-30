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

    public async Task<IEnumerable<CardExpense>> GetCardExpensesByCreditCardIdAsync(Guid creditCardId)
    {
        return await Entities
            .Where(ce => ce.CreditCardId == creditCardId)
            .Include(ce => ce.BankAccount)
            .Include(ce => ce.CreditCard)
            .Include(ce => ce.Invoice)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<CardExpense>> GetCardExpensesByInvoiceIdAsync(Guid invoiceId)
    {
        return await Entities
            .Where(ce => ce.InvoiceId == invoiceId)
            .Include(ce => ce.BankAccount)
            .Include(ce => ce.CreditCard)
            .Include(ce => ce.Invoice)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<CardExpense>> GetCardExpensesByPeriodAsync(DateTime startDate, DateTime endDate)
    {
        return await Entities
            .Where(ce => ce.Date >= startDate && ce.Date <= endDate)
            .Include(ce => ce.BankAccount)
            .Include(ce => ce.CreditCard)
            .Include(ce => ce.Invoice)
            .AsNoTracking()
            .ToListAsync();
    }
}