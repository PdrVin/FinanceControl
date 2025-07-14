using Application.DTOs.CreditCards;
using Application.DTOs.CardExpenses;

namespace Application.DTOs.Invoices;

public class InvoiceResponse
{
    public Guid Id { get; set; }
    public string InvoiceName { get; set; } = string.Empty;
    public DateTime ClosingDate { get; set; }
    public DateTime DueDate { get; set; }
    public decimal TotalAmount { get; set; }
    public int ReferenceMonth { get; set; }
    public int ReferenceYear { get; set; }
    public bool IsPaid { get; set; }
    public Guid CreditCardId { get; set; }
    public CreditCardResponse? CreditCard { get; set; }
    public ICollection<CardExpenseResponse>? CardExpenses { get; set; }

}