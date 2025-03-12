namespace Domain.Entities;

public class Invoice
{
    public int Id { get; set; }
    public required string CardName { get; set; }
    public DateTime ClosingDate { get; set; }
    public DateTime DueDate { get; set; }
    public decimal TotalAmount { get; set; }
    public ICollection<Expense>? Expenses { get; set; }
}