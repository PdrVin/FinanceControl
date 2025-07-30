using Application.DTOs.Transfers;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;

[Route("Transfers")]
public class TransferController : Controller
{
    private readonly ITransferService _transferService;
    private readonly IBankAccountService _bankAccountService;

    public TransferController(ITransferService transferService, IBankAccountService bankAccountService)
    {
        _transferService = transferService;
        _bankAccountService = bankAccountService;
    }

    // GET: /Transfers
    public async Task<IActionResult> Index([FromQuery] Guid? accountId)
    {
        IEnumerable<TransferResponse> transfers;
        if (accountId.HasValue)
        {
            transfers = await _transferService.GetTransfersByAccountIdAsync(accountId.Value);
            ViewBag.BankAccountId = accountId.Value;
        }
        else
        {
            transfers = new List<TransferResponse>();
        }
        
        ViewBag.BankAccounts = await _bankAccountService.GetAllBankAccountsAsync();
        return View(transfers);
    }
    
    // GET: /Transfers/Details/5
    [HttpGet("Details/{id:guid}")]
    public async Task<IActionResult> Details(Guid id)
    {
        try
        {
            var transfer = await _transferService.GetTransferByIdAsync(id);
            return View(transfer);
        }
        catch (KeyNotFoundException)
        {
            TempData["ErrorMessage"] = $"Transferência com o ID '{id}' não foi encontrada.";
            return RedirectToAction(nameof(Index));
        }
    }

    // GET: /Transfers/Create
    [HttpGet("Create")]
    public async Task<IActionResult> Create()
    {
        ViewBag.BankAccounts = await _bankAccountService.GetAllBankAccountsAsync();
        return View();
    }

    // POST: /Transfers/Create
    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([FromForm] TransferRequest request)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.BankAccounts = await _bankAccountService.GetAllBankAccountsAsync();
            return View(request);
        }

        try
        {
            await _transferService.CreateTransferAsync(request);
            TempData["SuccessMessage"] = "Transferência realizada com sucesso!";
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

        ViewBag.BankAccounts = await _bankAccountService.GetAllBankAccountsAsync();
        return View(request);
    }
    
    // POST: /Transfers/Delete/5
    [HttpPost("Delete/{id:guid}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        try
        {
            await _transferService.DeleteTransferAsync(id);
            TempData["SuccessMessage"] = "Transferência excluída com sucesso!";
            return RedirectToAction(nameof(Index));
        }
        catch (KeyNotFoundException)
        {
            TempData["ErrorMessage"] = $"Transferência com o ID '{id}' não foi encontrada.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Ocorreu um erro ao deletar: {ex.Message}";
            return RedirectToAction(nameof(Details), new { id = id });
        }
    }
}