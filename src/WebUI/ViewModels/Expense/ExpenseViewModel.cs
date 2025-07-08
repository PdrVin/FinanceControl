using Application.DTOs;

namespace WebUI.ViewModels.Expense;

public class ExpenseViewModel
{
    public IEnumerable<ExpenseDto> Expenses { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public string SearchTerm { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
}