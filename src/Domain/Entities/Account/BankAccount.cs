using Domain.Entities.Base;

namespace Domain.Entities;

public class BankAccount : EntityBase
{
    public string Name { get; set; }
    public decimal InitialBalance { get; set; }
    public decimal CurrentBalance { get; set; }
    public decimal ExpectedBalance { get; set; }
    public bool IsActive { get; set; }
    public ICollection<CardExpense> CardExpenses { get; set; }
    public ICollection<Expense> Expenses { get; set; }
    public ICollection<Income> Incomes { get; set; }
    public ICollection<Transfer> Transfers { get; set; }
}
