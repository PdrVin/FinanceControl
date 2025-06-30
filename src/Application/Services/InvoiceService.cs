using Application.DTOs;
using Application.Interfaces;
using Application.Services.Base;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Base;
using AutoMapper;
using Application.Helpers.Pagination;

namespace Application.Services;

public class InvoiceService : Service<InvoiceDto, Invoice>, IInvoiceService
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public InvoiceService(
        IInvoiceRepository repository,
        IUnitOfWork unitOfWork,
        IMapper mapper
    )
        : base(repository, unitOfWork, mapper)
    {
        _invoiceRepository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    #region GET
    public async Task<IEnumerable<Invoice>> GetAllInvoicesAsync()
    {
        return await _invoiceRepository.GetAllInvoicesAsync();
    }

    public async Task<Invoice?> GetInvoiceByIdAsync(Guid id)
    {
        return await _invoiceRepository.GetInvoiceByIdAsync(id);
    }
    #endregion

    #region Add
    public async Task AddInvoiceAsync(InvoiceDto invoiceDto)
    {
        var invoice = _mapper.Map<Invoice>(invoiceDto);

        await _invoiceRepository.SaveAsync(invoice);
        await _unitOfWork.CommitAsync();
    }
    #endregion

    #region Update
    public async Task UpdateInvoiceAsync(InvoiceDto editInvoice)
    {
        Guid? id = editInvoice.Id
            ?? throw new ArgumentException("Invoice ID cannot be null.", nameof(editInvoice.Id));

        Invoice invoice = await _invoiceRepository.GetByIdAsync(id.Value)
            ?? throw new KeyNotFoundException($"Invoice with ID '{id}' not found.");

        invoice = _mapper.Map(editInvoice, invoice);

        _invoiceRepository.Update(invoice);
        await _unitOfWork.CommitAsync();
    }
    #endregion

    #region Paginate
    public async Task<PagedResult<InvoiceDto>> GetPaginatedInvoicesAsync(
        int pageNumber, int pageSize, string searchTerm = "")
    {
        var (invoices, totalItems) = await _invoiceRepository
            .GetPaginatedAsync(pageNumber, pageSize, searchTerm);

        var invoiceDTOs = _mapper.Map<IEnumerable<InvoiceDto>>(invoices);

        return new PagedResult<InvoiceDto>
        {
            Items = invoiceDTOs,
            TotalItems = totalItems,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }
    #endregion
}
