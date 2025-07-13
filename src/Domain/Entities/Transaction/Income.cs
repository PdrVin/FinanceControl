using Domain.Entities.Base;
using Domain.Enums;

namespace Domain.Entities;

public class Income : EntityBase
{
    public string Description { get; set; }
    public decimal Amount { get; set; }
    public PayType PayType { get; set; }
    public DateTime Date { get; set; }
    public IncomeCategory Category { get; set; }
    public bool IsReceived { get; set; }

    public Guid? BankAccountId { get; set; }
    public BankAccount? BankAccount { get; set; }

    protected Income() { }

    public Income
    (
        string description,
        decimal amount,
        IncomeCategory category,
        DateTime date,
        PayType type = PayType.Pix,
        bool isReceived = false,
        Guid? bankAccountId = null
    )
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("The description cannot be null or empty.", nameof(description));

        if (amount <= 0)
            throw new ArgumentException("Amount must be greater than zero.", nameof(amount));

        if (!Enum.IsDefined(typeof(IncomeCategory), category))
            throw new ArgumentException("Invalid category.", nameof(category));

        if (date == default)
            throw new ArgumentException("The date provided is invalid.", nameof(date));

        Description = description;
        Amount = amount;
        PayType = type;
        Category = category;
        Date = date;
        IsReceived = isReceived;
        BankAccountId = bankAccountId;
    }
}
