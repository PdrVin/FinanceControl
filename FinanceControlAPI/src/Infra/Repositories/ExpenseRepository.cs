using Application.Interfaces;
using Domain.Entities;
using Infra.Context;
using Infra.Repositories.Base;

namespace Infra.Repositories;

public class ExpenseRepository(FinanceDbContext context)
    : Repository<Expense>(context), IExpenseRepository
{ }
