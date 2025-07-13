using Domain.Entities.Base;
using Domain.Enums;

namespace Domain.Entities;

public class CardExpense : EntityBase
{
    public string Description { get; set; }
    public decimal Amount { get; set; }
    public PayType PayType { get; set; }
    public DateTime Date { get; set; }
    public ExpenseCategory Category { get; set; }
    public bool IsPaid { get; set; }

    public Guid CreditCardId { get; set; }
    public CreditCard CreditCard { get; set; }

    public Guid? InvoiceId { get; set; }
    public Invoice? Invoice { get; set; }


    protected CardExpense() { }

    public CardExpense
    (
        string description,
        decimal amount,
        ExpenseCategory category,
        DateTime date,
        Guid creditCardId,
        PayType type = PayType.Pix,
        bool isPaid = false,
        Guid? invoiceId = null
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
        CreditCardId = creditCardId;
        InvoiceId = invoiceId;
    }
}