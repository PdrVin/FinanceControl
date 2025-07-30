using Application.DTOs.Expenses;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebUI.ViewModels.Expense;

namespace WebUI.Controllers;

[Route("Despesas")]
public class ExpenseController : Controller
{
    private readonly IExpenseService _expenseService;
    private readonly IBankAccountService _bankAccountService;

    public ExpenseController(IExpenseService expenseService, IBankAccountService bankAccountService)
    {
        _expenseService = expenseService;
        _bankAccountService = bankAccountService;
    }

    // public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10, string searchTerm = "")
    // {
    //     var result = await _expenseService.GetPaginatedExpensesAsync(pageNumber, pageSize, searchTerm);
    //     return View(
    //         new ExpenseViewModel
    //         {
    //             Expenses = result.Items,
    //             TotalItems = result.TotalItems,
    //             PageNumber = result.PageNumber,
    //             PageSize = result.PageSize,
    //             SearchTerm = searchTerm
    //         }
    //     );
    // }

    // // GET: /Expenses/Details/5
    // [HttpGet("Details/{id:guid}")]
    // public async Task<IActionResult> Details(Guid id)
    // {
    //     try
    //     {
    //         var expense = await _expenseService.GetExpenseByIdAsync(id);
    //         return View(expense);
    //     }
    //     catch (KeyNotFoundException)
    //     {
    //         TempData["ErrorMessage"] = "Despesa não encontrada.";
    //         return RedirectToAction(nameof(Index));
    //     }
    // }

    // GET: /Expenses/Create
    [HttpGet("Create")]
    public async Task<IActionResult> Create()
    {
        ViewBag.BankAccounts = await _bankAccountService.GetAllBankAccountsAsync();
        return View();
    }

    // POST: /Expenses/Create
    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([FromForm] ExpenseRequest request)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.BankAccounts = await _bankAccountService.GetAllBankAccountsAsync();
            return View(request);
        }

        try
        {
            await _expenseService.CreateExpenseAsync(request);
            TempData["SuccessMessage"] = "Despesa criada com sucesso!";
            return RedirectToAction(nameof(Index));
        }
        catch (KeyNotFoundException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Ocorreu um erro ao criar a despesa: {ex.Message}";
        }

        ViewBag.BankAccounts = await _bankAccountService.GetAllBankAccountsAsync();
        return View(request);
    }

    // POST: /Expenses/Delete/5
    [HttpPost("Delete/{id:guid}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        try
        {
            await _expenseService.DeleteExpenseAsync(id);
            TempData["SuccessMessage"] = "Despesa excluída com sucesso!";
            return RedirectToAction(nameof(Index));
        }
        catch (KeyNotFoundException)
        {
            TempData["ErrorMessage"] = "Despesa não encontrada.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Ocorreu um erro ao deletar: {ex.Message}";
            return RedirectToAction(nameof(Index));
        }
    }
}