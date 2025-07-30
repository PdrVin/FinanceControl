using Application.DTOs.Incomes;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;

[Route("Receitas")]
public class IncomeController : Controller
{
    private readonly IIncomeService _incomeService;
    private readonly IBankAccountService _bankAccountService;

    public IncomeController(IIncomeService incomeService, IBankAccountService bankAccountService)
    {
        _incomeService = incomeService;
        _bankAccountService = bankAccountService;
    }

    // GET: /Incomes
    public async Task<IActionResult> Index([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string searchTerm = "")
    {
        var result = await _incomeService.GetPaginatedIncomesAsync(pageNumber, pageSize, searchTerm);
        ViewBag.TotalCount = result.Items.Count();
        ViewBag.PageNumber = pageNumber;
        ViewBag.PageSize = pageSize;
        ViewBag.SearchTerm = searchTerm;
        return View(result.Items);
    }
    
    // GET: /Incomes/Details/5
    [HttpGet("Details/{id:guid}")]
    public async Task<IActionResult> Details(Guid id)
    {
        try
        {
            var income = await _incomeService.GetIncomeByIdAsync(id);
            return View(income);
        }
        catch (KeyNotFoundException)
        {
            TempData["ErrorMessage"] = "Receita não encontrada.";
            return RedirectToAction(nameof(Index));
        }
    }

    // GET: /Incomes/Create
    [HttpGet("Create")]
    public async Task<IActionResult> Create()
    {
        ViewBag.BankAccounts = await _bankAccountService.GetAllBankAccountsAsync();
        return View();
    }

    // POST: /Incomes/Create
    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([FromForm] IncomeRequest request)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.BankAccounts = await _bankAccountService.GetAllBankAccountsAsync();
            return View(request);
        }

        try
        {
            await _incomeService.CreateIncomeAsync(request);
            TempData["SuccessMessage"] = "Receita criada com sucesso!";
            return RedirectToAction(nameof(Index));
        }
        catch (KeyNotFoundException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Ocorreu um erro ao criar a receita: {ex.Message}";
        }

        ViewBag.BankAccounts = await _bankAccountService.GetAllBankAccountsAsync();
        return View(request);
    }
    
    // POST: /Incomes/Delete/5
    [HttpPost("Delete/{id:guid}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        try
        {
            await _incomeService.DeleteIncomeAsync(id);
            TempData["SuccessMessage"] = "Receita excluída com sucesso!";
            return RedirectToAction(nameof(Index));
        }
        catch (KeyNotFoundException)
        {
            TempData["ErrorMessage"] = "Receita não encontrada.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Ocorreu um erro ao deletar: {ex.Message}";
            return RedirectToAction(nameof(Index));
        }
    }
}