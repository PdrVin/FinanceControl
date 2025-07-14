using Domain.Entities;
using Domain.Interfaces.Base;

namespace Domain.Interfaces;

public interface ICardExpenseRepository : IRepository<CardExpense>
{
    Task<IEnumerable<CardExpense>> GetExpensesByCreditCardIdAsync(Guid creditCardId);
    Task<IEnumerable<CardExpense>> GetExpensesByInvoiceIdAsync(Guid invoiceId);
}