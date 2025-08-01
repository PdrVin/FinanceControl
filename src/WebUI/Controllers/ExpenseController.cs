using Application.DTOs.Expenses;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
// using WebUI.ViewModels.Expense;

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

    #region Create
    [HttpGet("Create")]
    public async Task<IActionResult> Create()
    {
        ViewBag.BankAccounts = await _bankAccountService.GetAllBankAccountsAsync();
        return View();
    }

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
            return RedirectToAction(nameof(Index), "Transaction");
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
    #endregion

    #region Edit
        [HttpGet("Edit/{id:guid}")]
    public async Task<IActionResult> Edit(Guid id)
    {
        try
        {
            var expense = await _expenseService.GetExpenseByIdAsync(id);
            ViewBag.BankAccounts = await _bankAccountService.GetAllBankAccountsAsync();
            return View(expense);
        }
        catch (KeyNotFoundException)
        {
            TempData["ErrorMessage"] = "Despesa não encontrada.";
            return RedirectToAction(nameof(Index), "Transaction");
        }
    }

    [HttpPost("Edit/{id:guid}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, [FromForm] ExpenseRequest request)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.BankAccounts = await _bankAccountService.GetAllBankAccountsAsync();
            return View(request);
        }

        try
        {
            await _expenseService.UpdateExpenseAsync(id, request);
            TempData["SuccessMessage"] = "Despesa atualizada com sucesso!";
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
            var expense = await _expenseService.GetExpenseByIdAsync(id);
            return View(nameof(DeleteConfirmed), expense);
        }
        catch (KeyNotFoundException)
        {
            TempData["ErrorMessage"] = "Despesa não encontrada.";
            return RedirectToAction(nameof(Index), "Transaction");
        }
    }

    [HttpPost("Delete/{id:guid}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        try
        {
            await _expenseService.DeleteExpenseAsync(id);
            TempData["SuccessMessage"] = "Despesa excluída com sucesso!";
        }
        catch (KeyNotFoundException)
        {
            TempData["ErrorMessage"] = "Despesa não encontrada.";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Ocorreu um erro ao deletar: {ex.Message}";
        }
        return RedirectToAction(nameof(Index), "Transaction");
    }
    #endregion
}