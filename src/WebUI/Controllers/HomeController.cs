using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebUI.ViewModels.Dashboard; // Novo namespace

namespace WebUI.Controllers;

[Route("Dashboard")]
public class HomeController : Controller
{
    private readonly IBankAccountService _bankAccountService;
    private readonly IIncomeService _incomeService;
    private readonly IExpenseService _expenseService;
    private readonly ICardExpenseService _cardExpenseService;
    private readonly IInvoiceService _invoiceService;

    public HomeController(
        IBankAccountService bankAccountService,
        IIncomeService incomeService,
        IExpenseService expenseService,
        ICardExpenseService cardExpenseService,
        IInvoiceService invoiceService)
    {
        _bankAccountService = bankAccountService;
        _incomeService = incomeService;
        _expenseService = expenseService;
        _cardExpenseService = cardExpenseService;
        _invoiceService = invoiceService;
    }

    public async Task<IActionResult> Index([FromQuery] int? year, [FromQuery] int? month)
    {
        var currentYear = year ?? DateTime.Now.Year;
        var currentMonth = month ?? DateTime.Now.Month;

        ViewBag.CurrentYear = currentYear;
        ViewBag.CurrentMonth = currentMonth;
        ViewBag.Months = Enumerable.Range(1, 12)
            .Select(m => new { Value = m, Text = new DateTime(currentYear, m, 1).ToString("MMMM") });
        ViewBag.Years = Enumerable.Range(DateTime.Now.Year - 5, 10);

        // --- Dashboard ---

        // Dados base
        var bankAccounts = await _bankAccountService.GetAllBankAccountsAsync();
        var allIncomes = await _incomeService.GetAllIncomesAsync();
        var allExpenses = await _expenseService.GetAllExpensesAsync();
        var allCardExpenses = await _cardExpenseService.GetExpensesByCreditCardIdAsync(Guid.Empty);

        // 1. Saldo Atual
        var currentBalance = bankAccounts.Sum(a => a.CurrentBalance);

        // 2. Totais Mensais para o Gráfico de Barras e Balanço
        var monthlyIncomes = allIncomes
            .Where(i => i.Date.Year == currentYear && i.Date.Month == currentMonth).Sum(i => i.Amount);

        var monthlyExpenses = allExpenses
            .Where(e => e.Date.Year == currentYear && e.Date.Month == currentMonth).Sum(e => e.Amount);

        var monthlyCardExpenses = allCardExpenses
            .Where(ce => ce.Date.Year == currentYear && ce.Date.Month == currentMonth).Sum(ce => ce.Amount);

        var totalMonthlyExpenses = monthlyExpenses + monthlyCardExpenses;
        var monthlyBalance = monthlyIncomes - totalMonthlyExpenses;

        // 3. Gráficos de Rosca por Categoria
        var incomeByCategory = allIncomes
            .Where(i => i.Date.Year == currentYear && i.Date.Month == currentMonth)
            .GroupBy(i => i.Category)
            .ToDictionary(g => g.Key.ToString(), g => g.Sum(i => i.Amount));

        var expenseByCategory = allExpenses
            .Where(e => e.Date.Year == currentYear && e.Date.Month == currentMonth)
            .GroupBy(e => e.Category)
            .ToDictionary(g => g.Key.ToString(), g => g.Sum(e => e.Amount));

        var cardExpenseByCategory = allCardExpenses
            .Where(ce => ce.Date.Year == currentYear && ce.Date.Month == currentMonth)
            .GroupBy(ce => ce.Category)
            .ToDictionary(g => g.Key.ToString(), g => g.Sum(ce => ce.Amount));

        foreach (var item in cardExpenseByCategory)
        {
            if (expenseByCategory.ContainsKey(item.Key))
                expenseByCategory[item.Key] += item.Value;
            else
                expenseByCategory[item.Key] = item.Value;
            
        }

        // 4. Balanço Mensal dos últimos 6 meses
        var balancesLastSixMonths = new List<MonthlyBalanceDto>();
        for (int i = 0; i < 6; i++)
        {
            var date = new DateTime(currentYear, currentMonth, 1).AddMonths(-i);

            var incomes = allIncomes
                .Where(inc => inc.Date.Year == date.Year && inc.Date.Month == date.Month).Sum(inc => inc.Amount);

            var expenses = allExpenses
                .Where(exp => exp.Date.Year == date.Year && exp.Date.Month == date.Month).Sum(exp => exp.Amount);

            var cardExpenses = allCardExpenses
                .Where(ce => ce.Date.Year == date.Year && ce.Date.Month == date.Month).Sum(ce => ce.Amount);

            var balance = incomes - (expenses + cardExpenses);
            balancesLastSixMonths.Add(new MonthlyBalanceDto
            {
                MonthYear = date.ToString("MMM/yy"),
                Balance = balance
            });
        }
        balancesLastSixMonths.Reverse();

        var dashboard = new DashboardViewModel
        {
            CurrentBalance = currentBalance,
            MonthlyIncome = monthlyIncomes,
            MonthlyExpense = totalMonthlyExpenses,
            MonthlyCardExpense = monthlyCardExpenses,
            MonthlyBalance = monthlyBalance,
            IncomeByCategory = incomeByCategory,
            ExpenseByCategory = expenseByCategory,
            BalancesLastSixMonths = balancesLastSixMonths,
            MonthlySummary = new MonthlySummaryDto
            {
                TotalIncomes = monthlyIncomes,
                TotalExpenses = totalMonthlyExpenses,
                MonthlyBalance = monthlyBalance
            }
        };

        return View(dashboard);
    }
}