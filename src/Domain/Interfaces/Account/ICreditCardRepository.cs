using Domain.Entities;
using Domain.Interfaces.Base;

namespace Domain.Interfaces;

public interface ICreditCardRepository : IRepository<CreditCard>
{
    Task<CreditCard?> GetCreditCardAsync(Guid id);
}
