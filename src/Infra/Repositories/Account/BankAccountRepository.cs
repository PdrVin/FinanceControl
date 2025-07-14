using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Interfaces;
using Infra.Context;
using Infra.Repositories.Base;

namespace Infra.Repositories;

public class BankAccountRepository : Repository<BankAccount>, IBankAccountRepository
{
    public BankAccountRepository(FinanceDbContext context) : base(context) { }

    public async Task<IEnumerable<BankAccount>> GetAllBankAccountsAsync()
    {
        return await Entities
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<BankAccount> GetBankAccountByIdAsync(Guid id)
    {
        return await Entities
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == id)
            ?? throw new KeyNotFoundException($"BankAccount with id {id} not found");
    }

    public async Task<BankAccount> GetBankAccountByNameAsync(string name)
    {
        return await Entities
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Name == name)
            ?? throw new KeyNotFoundException($"BankAccount with name {name} not found");
    }
}