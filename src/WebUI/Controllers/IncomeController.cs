using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;

public class IncomeController : Controller
{
    private readonly IIncomeService _incomeService;
    private readonly IMapper _mapper;

    public IncomeController(IIncomeService incomeService, IMapper mapper)
    {
        _incomeService = incomeService;
        _mapper = mapper;
    }

    #region Index
    [HttpGet]
    public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 20, string searchTerm = "")
    {
        var paginatedIncomes = await _incomeService.GetPaginatedIncomesAsync(pageNumber, pageSize, searchTerm);

        return View(
            paginatedIncomes
        // new IncomeListViewModel
        // {
        //     Items = paginatedIncomes.Items,
        //     PageNumber = paginatedIncomes.PageNumber,
        //     PageSize = paginatedIncomes.PageSize,
        //     TotalItems = paginatedIncomes.TotalItems,
        //     SearchTerm = searchTerm
        // }
        );
    }
    #endregion

    #region Details
    [HttpGet("Details/{id}")]
    public async Task<IActionResult> Details(Guid id)
    {
        var income = await _incomeService.GetIncomeByIdAsync(id);
        if (income is null)
        {
            TempData["ErrorMessage"] = "Income not found.";
            return RedirectToAction(nameof(Index));
        }
        return View(income);
    }
    #endregion

    #region Create
    [HttpGet]
    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(IncomeDto income)
    {
        if (!ModelState.IsValid) return View(income);

        try
        {
            await _incomeService.AddIncomeAsync(income);
            TempData["MessageSuccess"] = "Income created successfully!";
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
        var income = await _incomeService.GetByIdAsync(id);
        if (income is null)
        {
            TempData["ErrorMessage"] = "Income not found for editing.";
            return RedirectToAction(nameof(Index));
        }
        return View(income);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(IncomeDto income)
    {
        if (!ModelState.IsValid) return View(income);

        try
        {
            await _incomeService.UpdateIncomeAsync(income);
            TempData["SuccessMessage"] = "Income updated successfully!";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception)
        {
            ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
        }
        return View(income);
    }
    #endregion

    #region Delete
    [HttpGet("Delete/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var income = await _incomeService.GetByIdAsync(id);
        if (income is null)
        {
            TempData["ErrorMessage"] = "Income not found for deletion.";
            return RedirectToAction(nameof(Index));
        }
        return View(income);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(Guid id)
    {
        try
        {
            if (_incomeService.DeleteAsync(id).IsCompleted)
                TempData["MessageSuccess"] = "Income deleted successfully.";
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