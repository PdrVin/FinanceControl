using Application.DTOs.CreditCards;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;

[Route("Cartões")]
public class CreditCardController : Controller
{
    private readonly ICreditCardService _creditCardService;

    public CreditCardController(ICreditCardService creditCardService)
    {
        _creditCardService = creditCardService;
    }

    // GET: /CreditCards
    public async Task<IActionResult> Index()
    {
        var creditCards = await _creditCardService.GetAllCreditCardsAsync();
        return View(creditCards);
    }

    // GET: /CreditCards/Details/5
    [HttpGet("Details/{id:guid}")]
    public async Task<IActionResult> Details(Guid id)
    {
        try
        {
            var creditCard = await _creditCardService.GetCreditCardByIdAsync(id);
            return View(creditCard);
        }
        catch (KeyNotFoundException)
        {
            TempData["ErrorMessage"] = $"Cartão de crédito com o ID '{id}' não foi encontrado.";
            return RedirectToAction(nameof(Index));
        }
    }

    // GET: /CreditCards/Create
    [HttpGet("Create")]
    public IActionResult Create()
    {
        return View();
    }

    // POST: /CreditCards/Create
    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([FromForm] CreditCardRequest request)
    {
        if (!ModelState.IsValid)
        {
            return View(request);
        }

        try
        {
            await _creditCardService.CreateCreditCardAsync(request);
            TempData["SuccessMessage"] = "Cartão de crédito criado com sucesso!";
            return RedirectToAction(nameof(Index));
        }
        catch (KeyNotFoundException ex)
        {
            TempData["ErrorMessage"] = $"Erro: {ex.Message}";
            return RedirectToAction(nameof(Create), request);
        }
    }

    // GET: /CreditCards/Edit/5
    [HttpGet("Edit/{id:guid}")]
    public async Task<IActionResult> Edit(Guid id)
    {
        try
        {
            var creditCard = await _creditCardService.GetCreditCardByIdAsync(id);
            var request = new CreditCardRequest
            {
                Name = creditCard.Name,
                Limit = creditCard.Limit,
                ClosingDay = creditCard.ClosingDay,
                DueDay = creditCard.DueDay,
                BankAccountId = creditCard.BankAccountId
            };
            return View(request);
        }
        catch (KeyNotFoundException)
        {
            TempData["ErrorMessage"] = $"Cartão de crédito com o ID '{id}' não foi encontrado.";
            return RedirectToAction(nameof(Index));
        }
    }

    // POST: /CreditCards/Edit/5
    [HttpPost("Edit/{id:guid}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, [FromForm] CreditCardRequest request)
    {
        if (!ModelState.IsValid)
        {
            return View(request);
        }

        try
        {
            await _creditCardService.UpdateCreditCardAsync(id, request);
            TempData["SuccessMessage"] = "Cartão de crédito atualizado com sucesso!";
            return RedirectToAction(nameof(Index));
        }
        catch (KeyNotFoundException)
        {
            TempData["ErrorMessage"] = $"Cartão de crédito com o ID '{id}' não foi encontrado.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Ocorreu um erro ao atualizar: {ex.Message}";
            return View(request);
        }
    }

    // POST: /CreditCards/Delete/5
    [HttpPost("Delete/{id:guid}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        try
        {
            await _creditCardService.DeleteAsync(id);
            TempData["SuccessMessage"] = "Cartão de crédito excluído com sucesso!";
            return RedirectToAction(nameof(Index));
        }
        catch (KeyNotFoundException)
        {
            TempData["ErrorMessage"] = $"Cartão de crédito com o ID '{id}' não foi encontrado.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Ocorreu um erro ao deletar: {ex.Message}";
            return RedirectToAction(nameof(Details), new { id = id });
        }
    }
}