using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class IncomeController(IIncomeRepository incomeRepository) : ControllerBase
{
    private readonly IIncomeRepository _incomeRepository = incomeRepository;

    [HttpGet]
    public async Task<IActionResult> GetAll(int skip = 0, int take = 25)
    {
        var incomes = await _incomeRepository.GetAllAsync(skip, take);
        return Ok(incomes);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var income = await _incomeRepository.GetByIdAsync(id);
        return income is null ? NotFound() : Ok(income);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Income income)
    {
        if (income is null) return BadRequest("Income cannot be null");
        var result = await _incomeRepository.CreateAsync(income);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] Income income)
    {
        if (id != income.Id) return BadRequest("ID mismatch");

        var existingIncome = await _incomeRepository.GetByIdAsync(id);
        if (existingIncome is null) return NotFound();

        var result = await _incomeRepository.UpdateAsync(income);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var existingIncome = await _incomeRepository.GetByIdAsync(id);
        if (existingIncome is null) return NotFound();

        await _incomeRepository.DeleteAsync(id);
        return NoContent();
    }
}


