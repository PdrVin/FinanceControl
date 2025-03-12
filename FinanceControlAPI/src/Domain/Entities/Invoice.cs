using Domain.Enums;

namespace Domain.Entities;

public class Invoice
{
    public Guid Id { get; set; }
    public required string CardName { get; set; }
    public Account Account { get; set; }
    public DateTime ClosingDate { get; set; }
    public DateTime DueDate { get; set; }
    public decimal TotalAmount { get; set; }
    public ICollection<Expense>? Expenses { get; set; }
}