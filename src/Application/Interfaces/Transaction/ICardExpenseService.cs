using Application.DTOs.CardExpenses;
using Application.DTOs.Invoices;
using Application.Interfaces.Base;
using Domain.Entities;

namespace Application.Interfaces;

public interface ICardExpenseService : IService<CardExpenseRequest, CardExpenseResponse, CardExpense>
{
    Task<CardExpenseResponse> GetCardExpenseByIdAsync(Guid id);
    Task<IEnumerable<CardExpenseResponse>> GetExpensesByCreditCardIdAsync(Guid creditCardId);
    Task<IEnumerable<CardExpenseResponse>> GetExpensesByInvoiceIdAsync(Guid invoiceId);

    Task CreateCardExpenseAsync(CardExpenseRequest request);
    Task UpdateCardExpenseAsync(Guid id, CardExpenseRequest request);
}