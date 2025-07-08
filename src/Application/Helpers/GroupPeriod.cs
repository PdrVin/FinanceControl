using Application.DTOs;
using Domain.Entities.Base;

namespace Application.Helpers;

public class MonthlyExpenseGroup<TDto>
    where TDto : class
{
    public string MonthYear { get; set; }
    public IEnumerable<TDto> Entities { get; set; }
    public decimal MonthlyTotal { get; set; }
}