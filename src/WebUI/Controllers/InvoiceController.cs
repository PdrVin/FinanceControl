using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;

public class InvoiceController : Controller
{
    private readonly IInvoiceService _invoiceService;
    private readonly IMapper _mapper;

    public InvoiceController(IInvoiceService invoiceService, IMapper mapper)
    {
        _invoiceService = invoiceService;
        _mapper = mapper;
    }

    #region Index
    [HttpGet]
    public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 20, string searchTerm = "")
    {
        var paginatedInvoices = await _invoiceService.GetPaginatedInvoicesAsync(pageNumber, pageSize, searchTerm);

        return View(
            paginatedInvoices
        // new InvoiceListViewModel
        // {
        //     Items = paginatedInvoices.Items,
        //     PageNumber = paginatedInvoices.PageNumber,
        //     PageSize = paginatedInvoices.PageSize,
        //     TotalItems = paginatedInvoices.TotalItems,
        //     SearchTerm = searchTerm
        // }
        );
    }
    #endregion

    #region Details
    [HttpGet("Details/{id}")]
    public async Task<IActionResult> Details(Guid id)
    {
        var invoice = await _invoiceService.GetInvoiceByIdAsync(id);
        if (invoice is null)
        {
            TempData["ErrorMessage"] = "Invoice not found.";
            return RedirectToAction(nameof(Index));
        }
        return View(invoice);
    }
    #endregion

    #region Create
    [HttpGet]
    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(InvoiceDto invoice)
    {
        if (!ModelState.IsValid) return View(invoice);

        try
        {
            await _invoiceService.AddInvoiceAsync(invoice);
            TempData["MessageSuccess"] = "Invoice created successfully!";
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
        var invoice = await _invoiceService.GetByIdAsync(id);
        if (invoice is null)
        {
            TempData["ErrorMessage"] = "Invoice not found for editing.";
            return RedirectToAction(nameof(Index));
        }
        return View(invoice);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(InvoiceDto invoice)
    {
        if (!ModelState.IsValid) return View(invoice);

        try
        {
            await _invoiceService.UpdateInvoiceAsync(invoice);
            TempData["SuccessMessage"] = "Invoice updated successfully!";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception)
        {
            ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
        }
        return View(invoice);
    }
    #endregion

    #region Delete
    [HttpGet("Delete/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var invoice = await _invoiceService.GetByIdAsync(id);
        if (invoice is null)
        {
            TempData["ErrorMessage"] = "Invoice not found for deletion.";
            return RedirectToAction(nameof(Index));
        }
        return View(invoice);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(Guid id)
    {
        try
        {
            if (_invoiceService.DeleteAsync(id).IsCompleted)
                TempData["MessageSuccess"] = "Invoice deleted successfully.";
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