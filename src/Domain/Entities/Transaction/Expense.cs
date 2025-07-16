using Domain.Entities.Base;
using Domain.Enums;

namespace Domain.Entities;

public class Expense : EntityBase
{
    public string Description { get; set; }
    public decimal Amount { get; set; }
    public PayType PayType { get; set; }
    public DateTime Date { get; set; }
    public ExpenseCategory Category { get; set; }
    public bool IsPaid { get; set; }

    public Guid BankAccountId { get; set; }
    public BankAccount? BankAccount { get; set; }


    protected Expense() { }

    public Expense
    (
        string description,
        decimal amount,
        ExpenseCategory category,
        DateTime date,
        Guid bankAccountId,
        PayType type = PayType.Pix,
        bool isPaid = false
    )
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("The description cannot be null or empty.", nameof(description));

        if (amount <= 0)
            throw new ArgumentException("Amount must be greater than zero.", nameof(amount));

        if (!Enum.IsDefined(typeof(ExpenseCategory), category))
            throw new ArgumentException("Invalid category.", nameof(category));

        if (date == default)
            throw new ArgumentException("The date provided is invalid.", nameof(date));

        Description = description;
        Amount = amount;
        PayType = type;
        Category = category;
        Date = date;
        IsPaid = isPaid;
        BankAccountId = bankAccountId;
    }
}