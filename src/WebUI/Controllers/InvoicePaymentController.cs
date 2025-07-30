using Application.DTOs.InvoicePayments;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;

[Route("InvoicePayments")]
public class InvoicePaymentsController : Controller
{
    private readonly IInvoicePaymentService _invoicePaymentService;
    private readonly IBankAccountService _bankAccountService;
    private readonly IInvoiceService _invoiceService;

    public InvoicePaymentsController(
        IInvoicePaymentService invoicePaymentService,
        IBankAccountService bankAccountService,
        IInvoiceService invoiceService)
    {
        _invoicePaymentService = invoicePaymentService;
        _bankAccountService = bankAccountService;
        _invoiceService = invoiceService;
    }

    // GET: /InvoicePayments/ByInvoice/5
    [HttpGet("ByInvoice/{invoiceId:guid}")]
    public async Task<IActionResult> Index(Guid invoiceId)
    {
        var payments = await _invoicePaymentService.GetPaymentsByInvoiceIdAsync(invoiceId);
        ViewBag.InvoiceId = invoiceId;
        return View(payments);
    }
    
    // GET: /InvoicePayments/Create/5
    [HttpGet("Create/{invoiceId:guid}")]
    public async Task<IActionResult> Create(Guid invoiceId)
    {
        ViewBag.BankAccounts = await _bankAccountService.GetAllBankAccountsAsync();
        ViewBag.InvoiceId = invoiceId;
        return View();
    }

    // POST: /InvoicePayments/Create
    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([FromForm] InvoicePaymentRequest request)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.BankAccounts = await _bankAccountService.GetAllBankAccountsAsync();
            return View(request);
        }

        try
        {
            await _invoicePaymentService.CreateInvoicePaymentAsync(request);
            TempData["SuccessMessage"] = "Pagamento de fatura criado com sucesso!";
            return RedirectToAction(nameof(Index), new { invoiceId = request.InvoiceId });
        }
        catch (KeyNotFoundException ex)
        {
            TempData["ErrorMessage"] = $"Erro: {ex.Message}";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Ocorreu um erro ao criar: {ex.Message}";
        }
        
        ViewBag.BankAccounts = await _bankAccountService.GetAllBankAccountsAsync();
        return View(request);
    }
    
    // POST: /InvoicePayments/Delete/5
    [HttpPost("Delete/{id:guid}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        try
        {
            var payment = await _invoicePaymentService.GetInvoicePaymentByIdAsync(id);
            await _invoicePaymentService.DeleteInvoicePaymentAsync(id);
            TempData["SuccessMessage"] = "Pagamento de fatura excluído com sucesso!";
            return RedirectToAction(nameof(Index), new { invoiceId = payment.InvoiceId });
        }
        catch (KeyNotFoundException)
        {
            TempData["ErrorMessage"] = $"Pagamento com o ID '{id}' não foi encontrado.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Ocorreu um erro ao deletar: {ex.Message}";
            return RedirectToAction(nameof(Index));
        }
    }
}