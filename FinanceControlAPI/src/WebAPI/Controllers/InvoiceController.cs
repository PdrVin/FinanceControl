using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InvoiceController(IInvoiceRepository invoiceRepository)
    : ControllerBase
{
    private readonly IInvoiceRepository _invoiceRepository = invoiceRepository;

    [HttpGet]
    public async Task<IActionResult> GetAll(int skip = 0, int take = 25)
    {
        var invoices = await _invoiceRepository.GetAllAsync(skip, take);
        return Ok(invoices);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var invoice = await _invoiceRepository.GetByIdAsync(id);
        return invoice is null ? NotFound() : Ok(invoice);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Invoice invoice)
    {
        if (invoice is null) return BadRequest("Invoice cannot be null");
        var result = await _invoiceRepository.CreateAsync(invoice);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] Invoice invoice)
    {
        if (id != invoice.Id) return BadRequest("ID mismatch");

        var existingInvoice = await _invoiceRepository.GetByIdAsync(id);
        if (existingInvoice is null) return NotFound();

        var result = await _invoiceRepository.UpdateAsync(invoice);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var existingInvoice = await _invoiceRepository.GetByIdAsync(id);
        if (existingInvoice is null) return NotFound();

        await _invoiceRepository.DeleteAsync(id);
        return NoContent();
    }
}


