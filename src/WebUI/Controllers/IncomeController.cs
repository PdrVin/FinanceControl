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

    #region Create
    [HttpGet("Create")]
    public async Task<IActionResult> Create()
    {
        ViewBag.BankAccounts = await _bankAccountService.GetAllBankAccountsAsync();
        return View();
    }

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
            return RedirectToAction(nameof(Index), "Transaction");
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
    #endregion

    #region Edit
    [HttpGet("Edit/{id:guid}")]
    public async Task<IActionResult> Edit(Guid id)
    {
        try
        {
            var income = await _incomeService.GetIncomeByIdAsync(id);
            ViewBag.BankAccounts = await _bankAccountService.GetAllBankAccountsAsync();
            return View(income);
        }
        catch (KeyNotFoundException)
        {
            TempData["ErrorMessage"] = "Receita não encontrada.";
            return RedirectToAction(nameof(Index), "Transaction");
        }
    }

    [HttpPost("Edit/{id:guid}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, [FromForm] IncomeRequest request)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.BankAccounts = await _bankAccountService.GetAllBankAccountsAsync();
            return View(request);
        }

        try
        {
            await _incomeService.UpdateIncomeAsync(id, request);
            TempData["SuccessMessage"] = "Receita atualizada com sucesso!";
        }
        catch (KeyNotFoundException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Ocorreu um erro ao atualizar a receita: {ex.Message}";
        }
        return RedirectToAction(nameof(Index), "Transaction");
    }
    #endregion

    #region Delete
    [HttpGet("Delete/{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var income = await _incomeService.GetIncomeByIdAsync(id);
            return View(nameof(DeleteConfirmed), income);
        }
        catch (KeyNotFoundException)
        {
            TempData["ErrorMessage"] = "Receita não encontrada.";
            return RedirectToAction(nameof(Index), "Transaction");
        }
    }

    [HttpPost("Delete/{id:guid}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        try
        {
            await _incomeService.DeleteIncomeAsync(id);
            TempData["SuccessMessage"] = "Receita excluída com sucesso!";
        }
        catch (KeyNotFoundException)
        {
            TempData["ErrorMessage"] = "Receita não encontrada.";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Ocorreu um erro ao deletar: {ex.Message}";
        }
        return RedirectToAction(nameof(Index), "Transaction");
    }
    #endregion
}