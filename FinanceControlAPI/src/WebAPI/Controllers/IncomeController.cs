using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class IncomeController : ControllerBase
{
    private readonly IIncomeRepository _incomeRepository;

    public IncomeController(IIncomeRepository incomeRepository)
    {
        _incomeRepository = incomeRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var incomes = await _incomeRepository.GetAllIncomes();
        return Ok(incomes);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var income = await _incomeRepository.GetIncomeById(id);
        if (income == null) return NotFound();
        return Ok(income);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Income income)
    {
        await _incomeRepository.AddIncome(income);
        return CreatedAtAction(nameof(GetById), new { id = income.Id }, income);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] Income income)
    {
        if (id != income.Id) return BadRequest();
        await _incomeRepository.UpdateIncome(income);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _incomeRepository.DeleteIncome(id);
        return NoContent();
    }
}


