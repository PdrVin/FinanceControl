using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.BankAccounts;

public class BankAccountRequest
{
    [Required(ErrorMessage = "Bank account name is required.")]
    [StringLength(100, ErrorMessage = "Bank account name cannot exceed 100 characters.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Initial balance is required.")]
    [Range(0, double.MaxValue, ErrorMessage = "Initial balance must be a positive value.")]
    public decimal InitialBalance { get; set; }

    public bool IsActive { get; set; } = true;
}