using Domain.Entities.Base;

namespace Domain.Entities;

public class InvoicePayment : EntityBase
{
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }

    public Guid BankAccountId { get; set; }
    public BankAccount BankAccount { get; set; }

    public Guid InvoiceId { get; set; }
    public Invoice Invoice { get; set; }
}
