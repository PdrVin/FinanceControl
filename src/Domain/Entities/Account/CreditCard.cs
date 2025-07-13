using Domain.Entities.Base;

namespace Domain.Entities;

public class CreditCard : EntityBase
{
    public string Name { get; set; }
    public decimal Limit { get; set; }

    public int ClosingDay { get; set; }
    public int DueDay { get; set; }

    public Guid BankAccountId { get; set; }
    public BankAccount BankAccount { get; set; }

    public ICollection<CardExpense> CardExpenses { get; set; }
    public ICollection<Invoice> Invoices { get; set; }
}
