using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.InvoicePayments;

public class InvoicePaymentRequest
{
    [Required(ErrorMessage = "Amount is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
    public decimal Amount { get; set; }

    [Required(ErrorMessage = "Payment date is required.")]
    public DateTime PaymentDate { get; set; }

    [Required(ErrorMessage = "Bank Account ID is required.")]
    public Guid BankAccountId { get; set; }

    [Required(ErrorMessage = "Invoice ID is required.")]
    public Guid InvoiceId { get; set; }
}