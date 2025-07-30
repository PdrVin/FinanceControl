using Domain.Entities;
using Domain.Interfaces.Base;

namespace Domain.Interfaces;

public interface ICardExpenseRepository : IRepository<CardExpense>
{
    Task<IEnumerable<CardExpense>> GetCardExpensesByCreditCardIdAsync(Guid creditCardId);
    Task<IEnumerable<CardExpense>> GetCardExpensesByInvoiceIdAsync(Guid invoiceId);
    Task<IEnumerable<CardExpense>> GetCardExpensesByPeriodAsync(DateTime startDate, DateTime endDate);
}