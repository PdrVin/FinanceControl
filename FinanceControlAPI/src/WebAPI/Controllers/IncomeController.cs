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
    public async Task<IActionResult> GetAll()
    {
        var incomes = await _incomeRepository.GetAllAsync();
        return Ok(incomes);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var income = await _incomeRepository.GetByIdAsync(id);
        if (income == null) return NotFound();
        return Ok(income);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Income income)
    {
        await _incomeRepository.SaveAsync(income);
        return CreatedAtAction(nameof(GetById), new { id = income.Id }, income);
    }

    [HttpPut("{id}")]
    public IActionResult Update(Guid id, [FromBody] Income income)
    {
        if (id != income.Id) return BadRequest();
        _incomeRepository.Update(income);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id, [FromBody] Income income)
    {
        if (id != income.Id) return BadRequest();
        _incomeRepository.Delete(income);
        return NoContent();
    }
}


