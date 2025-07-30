using Application.DTOs.BankAccounts;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;

[Route("Contas")]
public class BankAccountController : Controller
{
    private readonly IBankAccountService _bankAccountService;

    public BankAccountController(IBankAccountService bankAccountService)
    {
        _bankAccountService = bankAccountService;
    }

    // GET: /BankAccounts
    // Adicione os parâmetros year e month aqui para o seletor
    public async Task<IActionResult> Index([FromQuery] int? year, [FromQuery] int? month)
    {
        // Garante que o ano e mês atuais sejam usados se não forem fornecidos na query string
        var currentYear = year ?? DateTime.Now.Year;
        var currentMonth = month ?? DateTime.Now.Month;

        ViewBag.CurrentYear = currentYear;
        ViewBag.CurrentMonth = currentMonth;
        
        // Populamos o ViewBag.BankAccounts para a paginação/filtros se necessário no futuro
        // Por enquanto, apenas para exibir as contas
        var bankAccounts = await _bankAccountService.GetAllBankAccountsAsync();
        
        return View(bankAccounts);
    }

    // GET: /BankAccounts/Details/5
    [HttpGet("Details/{id:guid}")]
    public async Task<IActionResult> Details(Guid id)
    {
        try
        {
            var bankAccount = await _bankAccountService.GetBankAccountByIdAsync(id);
            return View(bankAccount);
        }
        catch (KeyNotFoundException)
        {
            TempData["ErrorMessage"] = $"Conta bancária com o ID '{id}' não foi encontrada.";
            return RedirectToAction(nameof(Index));
        }
    }

    // GET: /BankAccounts/Create
    [HttpGet("Create")]
    public IActionResult Create()
    {
        return View();
    }

    // POST: /BankAccounts/Create
    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([FromForm] BankAccountRequest request)
    {
        if (!ModelState.IsValid)
        {
            return View(request);
        }

        try
        {
            await _bankAccountService.CreateBankAccountAsync(request);
            TempData["SuccessMessage"] = "Conta bancária criada com sucesso!";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Ocorreu um erro ao criar a conta: {ex.Message}";
            return View(request);
        }
    }

    // GET: /BankAccounts/Edit/5
    [HttpGet("Edit/{id:guid}")]
    public async Task<IActionResult> Edit(Guid id)
    {
        try
        {
            var bankAccount = await _bankAccountService.GetBankAccountByIdAsync(id);
            var request = new BankAccountRequest
            {
                Name = bankAccount.Name,
                InitialBalance = bankAccount.InitialBalance,
                IsActive = bankAccount.IsActive
            };
            return View(request);
        }
        catch (KeyNotFoundException)
        {
            TempData["ErrorMessage"] = $"Conta bancária com o ID '{id}' não foi encontrada.";
            return RedirectToAction(nameof(Index));
        }
    }

    // POST: /BankAccounts/Edit/5
    [HttpPost("Edit/{id:guid}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, [FromForm] BankAccountRequest request)
    {
        if (!ModelState.IsValid)
        {
            return View(request);
        }

        try
        {
            await _bankAccountService.UpdateBankAccountAsync(id, request);
            TempData["SuccessMessage"] = "Conta bancária atualizada com sucesso!";
            return RedirectToAction(nameof(Index));
        }
        catch (KeyNotFoundException)
        {
            TempData["ErrorMessage"] = $"Conta bancária com o ID '{id}' não foi encontrada.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Ocorreu um erro ao atualizar: {ex.Message}";
            return View(request);
        }
    }

    // POST: /BankAccounts/Delete/5
    [HttpPost("Delete/{id:guid}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        try
        {
            await _bankAccountService.DeleteAsync(id);
            TempData["SuccessMessage"] = "Conta bancária excluída com sucesso!";
            return RedirectToAction(nameof(Index));
        }
        catch (KeyNotFoundException)
        {
            TempData["ErrorMessage"] = $"Conta bancária com o ID '{id}' não foi encontrada.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Ocorreu um erro ao deletar: {ex.Message}";
            return RedirectToAction(nameof(Details), new { id = id });
        }
    }
}