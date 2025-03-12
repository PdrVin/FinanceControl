using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExpenseController(IExpenseRepository expenseRepository)
    : ControllerBase
{
    private readonly IExpenseRepository _expenseRepository = expenseRepository;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var expenses = await _expenseRepository.GetAllAsync();
        return Ok(expenses);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var expense = await _expenseRepository.GetByIdAsync(id);
        if (expense == null) return NotFound();
        return Ok(expense);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Expense expense)
    {
        await _expenseRepository.SaveAsync(expense);
        return CreatedAtAction(nameof(GetById), new { id = expense.Id }, expense);
    }

    [HttpPut("{id}")]
    public IActionResult Update(Guid id, [FromBody] Expense expense)
    {
        if (id != expense.Id) return BadRequest();
        _expenseRepository.Update(expense);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id, [FromBody] Expense expense)
    {
        if (id != expense.Id) return BadRequest();
        _expenseRepository.Delete(expense);
        return NoContent();
    }
}


