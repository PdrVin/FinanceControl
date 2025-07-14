using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.CreditCards;

public class CreditCardRequest
{
    [Required(ErrorMessage = "Credit card name is required.")]
    [StringLength(100, ErrorMessage = "Credit card name cannot exceed 100 characters.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Credit limit is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Credit limit must be greater than zero.")]
    public decimal Limit { get; set; }

    [Required(ErrorMessage = "Closing day is required.")]
    [Range(1, 31, ErrorMessage = "Closing day must be between 1 and 31.")]
    public int ClosingDay { get; set; }

    [Required(ErrorMessage = "Due day is required.")]
    [Range(1, 31, ErrorMessage = "Due day must be between 1 and 31.")]
    public int DueDay { get; set; }

    [Required(ErrorMessage = "Bank account ID is required.")]
    public Guid BankAccountId { get; set; }
}