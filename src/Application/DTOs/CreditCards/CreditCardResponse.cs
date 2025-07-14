using Application.DTOs.BankAccounts;
using Application.DTOs.CardExpenses;
using Application.DTOs.Invoices;

namespace Application.DTOs.CreditCards;

public class CreditCardResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Limit { get; set; }
    public int ClosingDay { get; set; }
    public int DueDay { get; set; }
    public Guid BankAccountId { get; set; }
    public BankAccountResponse? BankAccount { get; set; }
    public ICollection<CardExpenseResponse>? CardExpenses { get; set; }
    public ICollection<InvoiceResponse>? Invoices { get; set; }
}