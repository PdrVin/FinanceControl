using System.ComponentModel.DataAnnotations;
using Domain.Enums; // Assuming your enums are in Domain.Enums

namespace Application.DTOs.CardExpenses;

public class CardExpenseRequest
{
    [Required(ErrorMessage = "Description is required.")]
    [StringLength(250, ErrorMessage = "Description cannot exceed 250 characters.")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Amount is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
    public decimal Amount { get; set; }

    [Required(ErrorMessage = "Pay type is required.")]
    public PayType PayType { get; set; }

    [Required(ErrorMessage = "Date is required.")]
    public DateTime Date { get; set; }

    [Required(ErrorMessage = "Category is required.")]
    public ExpenseCategory Category { get; set; }

    public bool IsPaid { get; set; } = false;

    [Required(ErrorMessage = "Credit Card ID is required.")]
    public Guid CreditCardId { get; set; }

    [Required(ErrorMessage = "Invoice ID is required.")]
    public Guid InvoiceId { get; set; }
}