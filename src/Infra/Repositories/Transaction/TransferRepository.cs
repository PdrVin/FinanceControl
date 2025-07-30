using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Interfaces;
using Infra.Context;
using Infra.Repositories.Base;

namespace Infra.Repositories;

public class TransferRepository : Repository<Transfer>, ITransferRepository
{
    public TransferRepository(FinanceDbContext context) : base(context) { }

    public async Task<IEnumerable<Transfer>> GetTransfersByPeriodAsync(DateTime startDate, DateTime endDate)
    {
        return await Entities
            .Where(t => t.Date >= startDate && t.Date <= endDate)
            .Include(t => t.SourceAccount)
            .Include(t => t.DestinationAccount)
            .OrderByDescending(t => t.Date)
            .ToListAsync();
    }

    public async Task<IEnumerable<Transfer>> GetTransfersByAccountIdAsync(Guid accountId)
    {
        return await Entities
            .Where(t => t.SourceAccountId == accountId || t.DestinationAccountId == accountId)
            .Include(t => t.SourceAccount)
            .Include(t => t.DestinationAccount)
            .OrderByDescending(t => t.Date)
            .ToListAsync();
    }

    public async Task<IEnumerable<Transfer>> GetTransfersBetweenAccountsAsync(Guid account1Id, Guid account2Id)
    {
        return await Entities
            .Where(t => (t.SourceAccountId == account1Id && t.DestinationAccountId == account2Id) ||
                        (t.SourceAccountId == account2Id && t.DestinationAccountId == account1Id))
            .Include(t => t.SourceAccount)
            .Include(t => t.DestinationAccount)
            .OrderByDescending(t => t.Date)
            .ToListAsync();
    }
}