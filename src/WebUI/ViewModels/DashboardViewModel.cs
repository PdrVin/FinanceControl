namespace WebUI.ViewModels.Dashboard;

public class DashboardViewModel
{
    public decimal CurrentBalance { get; set; }
    public decimal MonthlyIncome { get; set; }
    public decimal MonthlyExpense { get; set; }
    public decimal MonthlyCardExpense { get; set; }
    public decimal MonthlyBalance { get; set; }
    public Dictionary<string, decimal> IncomeByCategory { get; set; } = new();
    public Dictionary<string, decimal> ExpenseByCategory { get; set; } = new();
    public List<MonthlyBalanceDto> BalancesLastSixMonths { get; set; } = new();
    public MonthlySummaryDto MonthlySummary { get; set; } = new();
}

public class MonthlySummaryDto
{
    public decimal TotalIncomes { get; set; }
    public decimal TotalExpenses { get; set; }
    public decimal MonthlyBalance { get; set; }
}

public class MonthlyBalanceDto
{
    public string? MonthYear { get; set; }
    public decimal Balance { get; set; }
}