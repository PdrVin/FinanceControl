using Domain.Entities;
using Domain.Interfaces.Base;

namespace Domain.Interfaces;

public interface IBankAccountRepository : IRepository<BankAccount>
{
    Task<IEnumerable<BankAccount>> GetAllBankAccountsAsync();
    Task<BankAccount> GetBankAccountByIdAsync(Guid id);
    Task<BankAccount> GetBankAccountByNameAsync(string name);
}