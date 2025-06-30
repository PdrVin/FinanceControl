using System.Text.Json.Serialization;
using Domain.Entities.Base;
using Domain.Enums;

namespace Domain.Entities;

public class Invoice : EntityBase
{
    public string InvoiceName { get; set; }
    public Account Account { get; set; }
    public DateTime ClosingDate { get; set; }
    public DateTime DueDate { get; set; }
    public decimal TotalAmount { get; set; }
    public ICollection<Expense>? Expenses { get; set; }

    protected Invoice() { }

    public Invoice(
        Account account,
        DateTime closingDate,
        DateTime dueDate,
        ICollection<Expense>? expenses = null
    )
    {
        if (closingDate >= dueDate)
            throw new ArgumentException("ClosingDate must be before the DueDate.", nameof(closingDate));

        Account = account;
        ClosingDate = closingDate;
        DueDate = dueDate;
        Expenses = expenses ?? new List<Expense>();
        UpdateInvoiceName();
        CalculateTotalAmount();
        CreatedAt = DateTime.UtcNow;
    }

    public void AddExpense(Expense expense)
    {
        ArgumentNullException.ThrowIfNull(expense);

        if (expense.InvoiceId != null && expense.InvoiceId != Id)
            throw new ArgumentException("A despesa pertence a outra fatura.", nameof(expense.InvoiceId));

        Expenses?.Add(expense);
        CalculateTotalAmount();
    }

    private void UpdateInvoiceName()
    {
        InvoiceName = $"{Account} - {DueDate:MMMM yyyy}";
    }
    
    private void CalculateTotalAmount()
    {
        TotalAmount = Expenses?.Sum(ex => ex.Amount) ?? 0;
    }
}
