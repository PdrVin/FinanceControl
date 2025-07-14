using Domain.Enums;
using Application.DTOs.CreditCards;
using Application.DTOs.Invoices;

namespace Application.DTOs.CardExpenses;

public class CardExpenseResponse
{
    public Guid Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public PayType PayType { get; set; }
    public DateTime Date { get; set; }
    public ExpenseCategory Category { get; set; }
    public bool IsPaid { get; set; }
    public Guid CreditCardId { get; set; }
    public CreditCardResponse? CreditCard { get; set; }
    public Guid? InvoiceId { get; set; }
    public InvoiceResponse? Invoice { get; set; }
}