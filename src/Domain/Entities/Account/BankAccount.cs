using Domain.Entities.Base;

namespace Domain.Entities;

public class BankAccount : EntityBase
{
    public string Name { get; set; }
    public decimal InitialBalance { get; set; }
    public decimal CurrentBalance { get; set; }
    public decimal ExpectedBalance { get; set; }
    public bool IsActive { get; set; }
}
