namespace Application.DTOs.BankAccounts;

public class BankAccountResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal InitialBalance { get; set; }
    public decimal CurrentBalance { get; set; }
    public decimal ExpectedBalance { get; set; }
    public bool IsActive { get; set; }
}