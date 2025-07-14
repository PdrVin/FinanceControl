using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Interfaces;
using Infra.Context;
using Infra.Repositories.Base;

namespace Infra.Repositories;

public class CreditCardRepository : Repository<CreditCard>, ICreditCardRepository
{
    public CreditCardRepository(FinanceDbContext context) : base(context) { }

    public async Task<CreditCard?> GetCreditCardAsync(Guid id)
    {
        return await Entities
            .Include(cc => cc.BankAccount)
            .Include(cc => cc.Invoices)
            .Include(cc => cc.CardExpenses)
            .AsNoTracking()
            .FirstOrDefaultAsync(cc => cc.Id == id);
    }
}