using Domain.Enums;

namespace Application.DTOs;

public class InvoiceDto
{
    public Guid? Id { get; set; }
    public string InvoiceName { get; set; }
    public Account Account { get; set; }
    public DateTime ClosingDate { get; set; }
    public DateTime DueDate { get; set; }
    public ICollection<ExpenseDto>? Expenses { get; set; }
}
