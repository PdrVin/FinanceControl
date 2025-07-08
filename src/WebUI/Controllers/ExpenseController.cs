using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Domain.Entities;
using AutoMapper;
using Application.DTOs;
using WebUI.ViewModels.Expense;

namespace WebAPI.Controllers;

public class ExpenseController : Controller
{
    private readonly IExpenseService _expenseService;
    private readonly IMapper _mapper;

    public ExpenseController(IExpenseService expenseService, IMapper mapper)
    {
        _expenseService = expenseService;
        _mapper = mapper;
    }

    #region Index
    [HttpGet]
    public async Task<IActionResult> Index(
        int pageNumber = 1, int pageSize = 20, string searchTerm = "")
    {
        var paginatedExpenses = await _expenseService.GetPaginatedExpensesAsync(pageNumber, pageSize, searchTerm);

        return View(
            new ExpenseViewModel
            {
                Expenses = paginatedExpenses.Items,
                PageNumber = paginatedExpenses.PageNumber,
                PageSize = paginatedExpenses.PageSize,
                TotalItems = paginatedExpenses.TotalItems,
                SearchTerm = searchTerm
            }
        );
    }
    #endregion

    #region Details
    [HttpGet("Details/{id}")]
    public async Task<IActionResult> Details(Guid id)
    {
        var expense = await _expenseService.GetExpenseByIdAsync(id);
        if (expense is null)
        {
            TempData["ErrorMessage"] = "Expense not found.";
            return RedirectToAction(nameof(Index));
        }
        return View(expense);
    }
    #endregion

    #region Create
    [HttpGet]
    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ExpenseDto expense)
    {
        if (!ModelState.IsValid) return View(expense);

        try
        {
            await _expenseService.AddExpenseAsync(expense);
            TempData["MessageSuccess"] = "Expense created successfully!";
        }
        catch (InvalidOperationException ex)
        {
            TempData["MessageError"] = $"Error: {ex.Message}";
        }
        catch (Exception ex)
        {
            TempData["MessageError"] = $"Unexpected error: {ex.Message}";
        }
        return RedirectToAction(nameof(Index));
    }
    #endregion

    #region Edit
    [HttpGet("Edit/{id}")]
    public async Task<IActionResult> Edit(Guid id)
    {
        var expense = await _expenseService.GetByIdAsync(id);
        if (expense is null)
        {
            TempData["ErrorMessage"] = "Expense not found for editing.";
            return RedirectToAction(nameof(Index));
        }
        return View(expense);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ExpenseDto expense)
    {
        if (!ModelState.IsValid) return View(expense);

        try
        {
            await _expenseService.UpdateExpenseAsync(expense);
            TempData["SuccessMessage"] = "Expense updated successfully!";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception)
        {
            ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
        }
        return View(expense);
    }
    #endregion

    #region Delete
    [HttpGet("Delete/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var expense = await _expenseService.GetByIdAsync(id);
        if (expense is null)
        {
            TempData["ErrorMessage"] = "Expense not found for deletion.";
            return RedirectToAction(nameof(Index));
        }
        return View(expense);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(Guid id)
    {
        try
        {
            if (_expenseService.DeleteAsync(id).IsCompleted)
                TempData["MessageSuccess"] = "Expense deleted successfully.";
            else
                TempData["MessageError"] = "Error during deletion process.";
        }
        catch (Exception error)
        {
            TempData["MessageError"] = $"Error during deletion process: {error.Message}";
        }
        return RedirectToAction(nameof(Index));
    }
    #endregion
}
