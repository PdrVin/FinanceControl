using Application.DTOs.BankAccounts;

namespace Application.DTOs.Transfers;

public class TransferResponse
{
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string? Notes { get; set; }
    public Guid SourceAccountId { get; set; }
    public BankAccountResponse? SourceAccount { get; set; }
    public Guid DestinationAccountId { get; set; }
    public BankAccountResponse? DestinationAccount { get; set; }
}