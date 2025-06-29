using Domain.Interfaces;
using Domain.Entities;
using Infra.Context;
using Infra.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories;

public class InvoiceRepository : Repository<Invoice>, IInvoiceRepository
{
    public InvoiceRepository(FinanceDbContext context) : base(context)
    { }

    public async Task<IEnumerable<Invoice>> GetAllInvoicesAsync()
    {
        return await Entities
            .Include(i => i.Expenses)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Invoice> GetInvoiceByIdAsync(Guid id)
    {
        return await Entities
            .Include(i => i.Expenses)
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.Id == id)
            ?? throw new KeyNotFoundException($"Invoice with id {id} not found");
    }

    public async Task<(IEnumerable<Invoice> Items, int TotalCount)> GetPaginatedAsync(
        int pageNumber,
        int pageSize,
        string searchTerm = ""
    )
    {
        var query = Entities
            .Include(i => i.Expenses)
            .AsNoTracking();

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(i =>
                i.InvoiceName.Contains(searchTerm) ||
                i.Account.ToString().Contains(searchTerm)
            );
        }
        
        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(i => i.ClosingDate.Year)
            .ThenByDescending(i => i.ClosingDate.Month)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }
}