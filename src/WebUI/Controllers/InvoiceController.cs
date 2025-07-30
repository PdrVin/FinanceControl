using Application.DTOs.Invoices;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;

[Route("Faturas")]
public class InvoiceController : Controller
{
    private readonly IInvoiceService _invoiceService;
    private readonly ICreditCardService _creditCardService;

    public InvoiceController(IInvoiceService invoiceService, ICreditCardService creditCardService)
    {
        _invoiceService = invoiceService;
        _creditCardService = creditCardService;
    }

    // GET: /Invoices
    public async Task<IActionResult> Index([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string searchTerm = "")
    {
        var result = await _invoiceService.GetPaginatedInvoicesAsync(pageNumber, pageSize, searchTerm);
        ViewBag.TotalCount = result.Items.Count();
        ViewBag.PageNumber = pageNumber;
        ViewBag.PageSize = pageSize;
        ViewBag.SearchTerm = searchTerm;
        return View(result.Items);
    }

    // GET: /Invoices/Details/5
    [HttpGet("Details/{id:guid}")]
    public async Task<IActionResult> Details(Guid id)
    {
        try
        {
            var invoice = await _invoiceService.GetInvoiceByIdAsync(id);
            return View(invoice);
        }
        catch (KeyNotFoundException)
        {
            TempData["ErrorMessage"] = $"Fatura com o ID '{id}' não foi encontrada.";
            return RedirectToAction(nameof(Index));
        }
    }

    // GET: /Invoices/Create
    [HttpGet("Create")]
    public async Task<IActionResult> Create()
    {
        ViewBag.CreditCards = await _creditCardService.GetAllCreditCardsAsync();
        return View();
    }

    // POST: /Invoices/Create
    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([FromForm] InvoiceRequest request)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.CreditCards = await _creditCardService.GetAllCreditCardsAsync();
            return View(request);
        }

        try
        {
            await _invoiceService.CreateInvoiceAsync(request);
            TempData["SuccessMessage"] = "Fatura criada com sucesso!";
            return RedirectToAction(nameof(Index));
        }
        catch (KeyNotFoundException ex)
        {
            TempData["ErrorMessage"] = $"Erro: {ex.Message}";
        }
        catch (InvalidOperationException ex)
        {
            TempData["ErrorMessage"] = $"Erro: {ex.Message}";
        }

        ViewBag.CreditCards = await _creditCardService.GetAllCreditCardsAsync();
        return View(request);
    }

    // POST: /Invoices/Delete/5
    [HttpPost("Delete/{id:guid}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        try
        {
            await _invoiceService.DeleteAsync(id);
            TempData["SuccessMessage"] = "Fatura excluída com sucesso!";
            return RedirectToAction(nameof(Index));
        }
        catch (KeyNotFoundException)
        {
            TempData["ErrorMessage"] = $"Fatura com o ID '{id}' não foi encontrada.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Ocorreu um erro ao deletar: {ex.Message}";
            return RedirectToAction(nameof(Details), new { id = id });
        }
    }
}