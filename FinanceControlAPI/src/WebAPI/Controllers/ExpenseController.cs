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
    public async Task<IActionResult> GetAll(int skip = 0, int take = 25)
    {
        var expenses = await _expenseRepository.GetAllAsync(skip, take);
        return Ok(expenses);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var expense = await _expenseRepository.GetByIdAsync(id);
        return expense is null ? NotFound() : Ok(expense);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Expense expense)
    {
        if (expense is null) return BadRequest("Expense cannot be null");
        var result = await _expenseRepository.CreateAsync(expense);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] Expense expense)
    {
        if (id != expense.Id) return BadRequest("ID mismatch");

        var existingExpense = await _expenseRepository.GetByIdAsync(id);
        if (existingExpense is null) return NotFound();

        var result = await _expenseRepository.UpdateAsync(expense);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var existingExpense = await _expenseRepository.GetByIdAsync(id);
        if (existingExpense is null) return NotFound();

        await _expenseRepository.DeleteAsync(id);
        return NoContent();
    }
}


