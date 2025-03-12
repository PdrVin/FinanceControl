using Application.Interfaces;
using Domain.Entities;
using Infra.Context;
using Infra.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories;

public class InvoiceRepository(FinanceDbContext context)
    : Repository<Invoice>(context), IInvoiceRepository
{ }