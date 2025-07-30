using Microsoft.AspNetCore.Mvc;
using Application.Interfaces.Transaction;

namespace WebUI.Controllers;

[Route("Transacoes")]
public class TransactionController : Controller
{
    private readonly ITransactionService _transactionService;

    public TransactionController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    public async Task<IActionResult> Index(int? year, int? month, int page = 1, int pageSize = 10)
    {
        int currentYear = year ?? DateTime.Now.Year;
        int currentMonth = month ?? DateTime.Now.Month;

        var transactionList = await _transactionService.GetTransactionsByMonthAndYearAsync(
            currentYear, currentMonth, page, pageSize
        );

        // Adiciona dados para o front-end usar no dropdown de mês/ano
        ViewBag.CurrentYear = currentYear;
        ViewBag.CurrentMonth = currentMonth;
        ViewBag.CurrentPage = transactionList.CurrentPage;
        ViewBag.TotalPages = transactionList.TotalPages;

        return View(transactionList);
    }
}