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
    public async Task<IActionResult> GetAll()
    {
        var Invoices = await _invoiceRepository.GetAllAsync();
        return Ok(Invoices);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var invoice = await _invoiceRepository.GetByIdAsync(id);
        if (invoice == null) return NotFound();
        return Ok(invoice);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Invoice invoice)
    {
        await _invoiceRepository.SaveAsync(invoice);
        return CreatedAtAction(nameof(GetById), new { id = invoice.Id }, invoice);
    }

    [HttpPut("{id}")]
    public IActionResult Update(Guid id, [FromBody] Invoice invoice)
    {
        if (id != invoice.Id) return BadRequest();
        _invoiceRepository.Update(invoice);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id, [FromBody] Invoice invoice)
    {
        if (id != invoice.Id) return BadRequest();
        _invoiceRepository.Delete(invoice);
        return NoContent();
    }
}


