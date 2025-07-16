using Application.DTOs.Invoices;
using Application.Helpers;
using Application.Interfaces;
using Application.Services.Base;
using Domain.Entities;
using Domain.Interfaces.Base;
using Domain.Interfaces;
using AutoMapper;

namespace Application.Services.Invoices;

public class InvoiceService
    : Service<InvoiceRequest, InvoiceResponse, Invoice>, IInvoiceService
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly ICreditCardRepository _creditCardRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public InvoiceService(
        IInvoiceRepository repository,
        ICreditCardRepository creditCardRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper
    )
        : base(repository, unitOfWork, mapper)
    {
        _invoiceRepository = repository;
        _creditCardRepository = creditCardRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<InvoiceResponse>> GetAllInvoicesAsync()
    {
        var invoices = await _invoiceRepository.GetAllInvoicesAsync();
        return _mapper.Map<IEnumerable<InvoiceResponse>>(invoices);
    }

    public async Task<InvoiceResponse> GetInvoiceByIdAsync(Guid id)
    {
        var invoice = await _invoiceRepository.GetInvoiceByIdAsync(id)
            ?? throw new KeyNotFoundException($"Invoice with id {id} not found.");

        return _mapper.Map<InvoiceResponse>(invoice);
    }

    public async Task CreateInvoiceAsync(InvoiceRequest request)
    {
        var creditCard = await _creditCardRepository.GetByIdAsync(request.CreditCardId)
            ?? throw new KeyNotFoundException($"Credit card with id {request.CreditCardId} not found.");

        var existingInvoice = await _invoiceRepository.GetInvoiceByPeriodAsync(
            request.ReferenceMonth, request.ReferenceYear)
            ?? throw new InvalidOperationException($"An invoice for this credit card in {request.ReferenceMonth}/{request.ReferenceYear} already exists.");

        var invoice = _mapper.Map<Invoice>(request);

        await _invoiceRepository.SaveAsync(invoice);
        await _unitOfWork.CommitAsync();
    }

    public async Task UpdateInvoiceAsync(Guid id, InvoiceRequest request)
    {
        var existingInvoice = await _invoiceRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Invoice with id {id} not found.");

        _mapper.Map(request, existingInvoice);

        _invoiceRepository.Update(existingInvoice);
        await _unitOfWork.CommitAsync();
    }

    public async Task DeleteInvoiceAsync(Guid id)
    {
        _invoiceRepository.Delete(id);
        await _unitOfWork.CommitAsync();
    }

    public async Task<IEnumerable<InvoiceResponse>> GetInvoicesByCreditCardIdAsync(Guid creditCardId)
    {
        var invoices = await _invoiceRepository.GetInvoicesByCreditCardIdAsync(creditCardId);
        return _mapper.Map<IEnumerable<InvoiceResponse>>(invoices);
    }

    public async Task<InvoiceResponse?> GetInvoiceByPeriodAsync(int month, int year)
    {
        var invoice = await _invoiceRepository.GetInvoiceByPeriodAsync(month, year);
        return _mapper.Map<InvoiceResponse?>(invoice);
    }

    public async Task<PagedResult<InvoiceResponse>> GetPaginatedInvoicesAsync(
        int pageNumber, int pageSize, string searchTerm = "")
    {
        var (invoices, totalItems) = await _invoiceRepository
            .GetPaginatedAsync(pageNumber, pageSize, searchTerm);

        var invoiceResponses = _mapper.Map<IEnumerable<InvoiceResponse>>(invoices);

        return new PagedResult<InvoiceResponse>
        {
            Items = invoiceResponses,
            TotalItems = totalItems,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }
}