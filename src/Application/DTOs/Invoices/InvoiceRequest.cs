using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Invoices;

public class InvoiceRequest
{
    [Required(ErrorMessage = "Closing date is required.")]
    public DateTime ClosingDate { get; set; }

    [Required(ErrorMessage = "Due date is required.")]
    public DateTime DueDate { get; set; }

    [Required(ErrorMessage = "Reference month is required.")]
    [Range(1, 12, ErrorMessage = "Reference month must be between 1 and 12.")]
    public int ReferenceMonth { get; set; }

    [Required(ErrorMessage = "Reference year is required.")]
    [Range(2000, 2100, ErrorMessage = "Reference year must be a valid year.")]
    public int ReferenceYear { get; set; }

    public bool IsPaid { get; set; } = false;

    [Required(ErrorMessage = "Credit Card ID is required.")]
    public Guid CreditCardId { get; set; }
}