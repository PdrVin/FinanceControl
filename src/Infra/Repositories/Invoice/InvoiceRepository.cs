using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Interfaces;
using Infra.Context;
using Infra.Repositories.Base;

namespace Infra.Repositories;

public class InvoiceRepository : Repository<Invoice>, IInvoiceRepository
{
    public InvoiceRepository(FinanceDbContext context) : base(context) { }

    public async Task<IEnumerable<Invoice>> GetAllInvoicesAsync()
    {
        return await Entities
            .Include(i => i.CreditCard)
            .Include(i => i.CardExpenses)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Invoice> GetInvoiceByIdAsync(Guid id)
    {
        return await Entities
            .Include(i => i.CreditCard)
            .Include(i => i.CardExpenses)
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.Id == id)
            ?? throw new KeyNotFoundException($"Invoice with id {id} not found");
    }

    public async Task<IEnumerable<Invoice>> GetInvoicesByCreditCardIdAsync(Guid creditCardId)
    {
        return await Entities
            .Where(i => i.CreditCardId == creditCardId)
            .Include(i => i.CreditCard)
            .Include(i => i.CardExpenses)
            .OrderByDescending(i => i.ReferenceYear)
            .ThenByDescending(i => i.ReferenceMonth)
            .ToListAsync();
    }

    public async Task<Invoice?> GetInvoiceByPeriodAsync(int month, int year)
    {
        return await Entities
            .Where(i => i.ReferenceMonth == month && i.ReferenceYear == year)
            .Include(i => i.CreditCard)
            .Include(i => i.CardExpenses)
            .FirstOrDefaultAsync();
    }

    public async Task<(IEnumerable<Invoice> Items, int TotalCount)> GetPaginatedAsync(
        int pageNumber,
        int pageSize,
        string searchTerm = ""
    )
    {
        var query = Entities
            .Include(i => i.CreditCard)
            .Include(i => i.CardExpenses)
            .AsNoTracking();

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(i =>
                i.InvoiceName.Contains(searchTerm) ||
                i.CreditCard.Name.Contains(searchTerm)
            );
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(i => i.ReferenceYear)
            .ThenByDescending(i => i.ReferenceMonth)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }
}