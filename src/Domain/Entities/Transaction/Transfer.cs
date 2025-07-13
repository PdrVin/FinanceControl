using Domain.Entities.Base;

namespace Domain.Entities;

public class Transfer : EntityBase
{
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }

    public Guid SourceAccountId { get; set; }
    public BankAccount SourceAccount { get; set; }

    public Guid DestinationAccountId { get; set; }
    public BankAccount DestinationAccount { get; set; }
}
