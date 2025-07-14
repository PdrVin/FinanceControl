using Domain.Entities;
using Domain.Interfaces.Base;

namespace Domain.Interfaces;

public interface ITransferRepository : IRepository<Transfer>
{
    Task<IEnumerable<Transfer>> GetTransfersByAccountIdAsync(Guid accountId);
    Task<IEnumerable<Transfer>> GetTransfersBetweenAccountsAsync(Guid account1Id, Guid account2Id);
}