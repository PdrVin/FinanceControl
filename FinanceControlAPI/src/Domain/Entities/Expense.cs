using Domain.Enums;

namespace Domain.Entities;

public class Expense
{
    public Guid Id { get; set; }
    public Status Status { get; set; } = Status.Pendente;
    public PayType Type { get; set; } = PayType.Pix;
    public required DateTime Date { get; set; }
    public required string Description { get; set; }
    public required Category Category { get; set; }
    public required Account Account { get; set; } = Account.PicPay;
    public decimal Amount { get; set; }
    public int? InvoiceId { get; set; }
    public Invoice? Invoice { get; set; }
}

