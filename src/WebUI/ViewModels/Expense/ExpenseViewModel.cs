using Application.DTOs.Expenses;

namespace WebUI.ViewModels.Expense;

public class ExpenseViewModel
{
    public IEnumerable<ExpenseResponse> Expenses { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public string SearchTerm { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
}