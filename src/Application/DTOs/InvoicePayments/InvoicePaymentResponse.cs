using Application.DTOs.BankAccounts;
using Application.DTOs.Invoices;

namespace Application.DTOs.InvoicePayments;

public class InvoicePaymentResponse
{
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public Guid BankAccountId { get; set; }
    public BankAccountResponse? BankAccount { get; set; }
    public Guid InvoiceId { get; set; }
    public InvoiceResponse? Invoice { get; set; }
}