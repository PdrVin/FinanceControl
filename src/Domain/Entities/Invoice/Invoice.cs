using Domain.Entities.Base;
using Domain.Enums;

namespace Domain.Entities;

public class Invoice : EntityBase
{
    public string InvoiceName { get; set; }
    public DateTime ClosingDate { get; set; }
    public DateTime DueDate { get; set; }
    public decimal TotalAmount { get; set; }
    public int ReferenceMonth { get; set; }
    public int ReferenceYear { get; set; }
    public bool IsPaid { get; set; }

    public Guid CreditCardId { get; set; }
    public CreditCard CreditCard { get; set; }

    public ICollection<CardExpense>? CardExpenses { get; set; }

    protected Invoice() { }

    public Invoice(
        DateTime closingDate,
        DateTime dueDate,
        int referenceMonth,
        int referenceYear,
        bool isPaid = false
    )
    {
        if (closingDate >= dueDate)
            throw new ArgumentException("ClosingDate must be before the DueDate.", nameof(closingDate));

        ClosingDate = closingDate;
        DueDate = dueDate;
        ReferenceMonth = referenceMonth;
        ReferenceYear = referenceYear;
        IsPaid = isPaid;
        CardExpenses = [];
        UpdateInvoiceName();
        CalculateTotalAmount();
        CreatedAt = DateTime.UtcNow;
    }

    public void AddExpense(CardExpense expense)
    {
        ArgumentNullException.ThrowIfNull(expense);

        if (expense.InvoiceId != null && expense.InvoiceId != Id)
            throw new ArgumentException("A despesa pertence a outra fatura.", nameof(expense.InvoiceId));

        CardExpenses?.Add(expense);
        CalculateTotalAmount();
    }

    private void UpdateInvoiceName()
    {
        InvoiceName = $"{CreditCard.Name} - {DueDate.Month} {ReferenceYear}";
    }
    
    private void CalculateTotalAmount()
    {
        TotalAmount = CardExpenses?.Sum(ex => ex.Amount) ?? 0;
    }
}
