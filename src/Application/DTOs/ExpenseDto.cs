using Domain.Enums;

namespace Application.DTOs;

public class ExpenseDto
{
    public Guid? Id { get; set; }
    public Status Status { get; set; }
    public PayType PayType { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; }
    public ExpenseCategory Category { get; set; }
    public Account Account { get; set; }
    public decimal Amount { get; set; }
    public Guid? InvoiceId { get; set; }
}
