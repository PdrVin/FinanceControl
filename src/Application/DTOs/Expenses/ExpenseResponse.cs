using Domain.Enums;
using Application.DTOs.BankAccounts;

namespace Application.DTOs.Expenses;

public class ExpenseResponse
{
    public Guid Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public PayType PayType { get; set; }
    public DateTime Date { get; set; }
    public ExpenseCategory Category { get; set; }
    public bool IsPaid { get; set; }
    public Guid? BankAccountId { get; set; }
    public BankAccountResponse? BankAccount { get; set; }
}