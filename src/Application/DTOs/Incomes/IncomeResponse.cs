using Domain.Enums;
using Application.DTOs.BankAccounts;

namespace Application.DTOs.Incomes;

public class IncomeResponse
{
    public Guid Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public PayType PayType { get; set; }
    public DateTime Date { get; set; }
    public IncomeCategory Category { get; set; }
    public bool IsReceived { get; set; }
    public Guid? BankAccountId { get; set; }
    public BankAccountResponse? BankAccount { get; set; }
}