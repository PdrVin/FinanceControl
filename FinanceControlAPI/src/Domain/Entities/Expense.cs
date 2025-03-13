using Domain.Entities.Base;
using Domain.Enums;

namespace Domain.Entities;

public class Expense : EntityBase
{
    public Status Status { get; set; }
    public PayType PayType { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; }
    public ExpenseCategory Category { get; set; }
    public Account Account { get; set; }
    public decimal Amount { get; set; }
    public Guid? InvoiceId { get; set; }
    public Invoice? Invoice { get; set; }

    protected Expense() { }

    public Expense
    (
        string description,
        ExpenseCategory category,
        decimal amount,
        DateTime date,
        Status status = Status.Pendente,
        PayType type = PayType.Pix,
        Account account = Account.PicPay,
        Guid? invoiceId = null,
        Invoice? invoice = null
    )
    {
        if (date == default)
            throw new ArgumentException("The date provided is invalid.", nameof(date));

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("The description cannot be null or empty.", nameof(description));

        if (!Enum.IsDefined(typeof(ExpenseCategory), category))
            throw new ArgumentException("Invalid category.", nameof(category));

        if (amount <= 0)
            throw new ArgumentException("Amount must be greater than zero.", nameof(amount));

        if (invoice != null && invoiceId == null)
            throw new ArgumentException("If Invoice is provided, InvoiceId must also be present.", nameof(invoiceId));

        Status = status;
        PayType = type;
        Date = date;
        Description = description;
        Category = category;
        Account = account;
        Amount = amount;
        InvoiceId = invoiceId;
        Invoice = invoice;
    }
}

