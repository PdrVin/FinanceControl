using Domain.Entities;

namespace Application.Interfaces;

public interface IIncomeRepository
{
    Task<IEnumerable<Income>> GetAllIncomes();
    Task<Income?> GetIncomeById(Guid id);
    Task AddIncome(Income income);
    Task UpdateIncome(Income income);
    Task DeleteIncome(Guid id);
}