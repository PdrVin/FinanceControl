using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Transfers;

public class TransferRequest
{
    [Required(ErrorMessage = "Amount is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
    public decimal Amount { get; set; }

    [Required(ErrorMessage = "Date is required.")]
    public DateTime Date { get; set; }

    [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters.")]
    public string? Notes { get; set; }

    [Required(ErrorMessage = "Source Account ID is required.")]
    public Guid SourceAccountId { get; set; }

    [Required(ErrorMessage = "Destination Account ID is required.")]
    public Guid DestinationAccountId { get; set; }
}