using Application.DTOs.CardExpenses;
using Application.Interfaces;
using Application.Services.Base;
using Domain.Entities;
using Domain.Interfaces.Base;
using Domain.Interfaces;
using AutoMapper;

namespace Application.Services.Transaction;

public class CardExpenseService
    : Service<CardExpenseRequest, CardExpenseResponse, CardExpense>, ICardExpenseService
{
    private readonly ICardExpenseRepository _cardExpenseRepository;
    private readonly ICreditCardRepository _creditCardRepository;
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CardExpenseService(
        ICardExpenseRepository repository,
        ICreditCardRepository creditCardRepository,
        IInvoiceRepository invoiceRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper
    )
        : base(repository, unitOfWork, mapper)
    {
        _cardExpenseRepository = repository;
        _creditCardRepository = creditCardRepository;
        _invoiceRepository = invoiceRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CardExpenseResponse> GetCardExpenseByIdAsync(Guid id)
    {
        var expense = await _cardExpenseRepository.GetByIdAsync(id);
        return _mapper.Map<CardExpenseResponse>(expense);
    }

    public async Task<IEnumerable<CardExpenseResponse>> GetExpensesByCreditCardIdAsync(Guid creditCardId)
    {
        var expenses = await _cardExpenseRepository.GetCardExpensesByCreditCardIdAsync(creditCardId);
        return _mapper.Map<IEnumerable<CardExpenseResponse>>(expenses);
    }

    public async Task<IEnumerable<CardExpenseResponse>> GetExpensesByInvoiceIdAsync(Guid invoiceId)
    {
        var expenses = await _cardExpenseRepository.GetCardExpensesByInvoiceIdAsync(invoiceId);
        return _mapper.Map<IEnumerable<CardExpenseResponse>>(expenses);
    }

    public async Task CreateCardExpenseAsync(CardExpenseRequest request)
    {
        // Lógica de negócio: Validar a existência do cartão de crédito
        var creditCard = await _creditCardRepository.GetByIdAsync(request.CreditCardId)
            ?? throw new KeyNotFoundException($"Credit card with id {request.CreditCardId} not found.");

        // Se a despesa for associada a uma fatura, validar a existência
        var invoice = await _invoiceRepository.GetByIdAsync(request.InvoiceId)
            ?? throw new KeyNotFoundException($"Invoice with id {request.InvoiceId} not found.");

        var expense = _mapper.Map<CardExpense>(request);

        await _cardExpenseRepository.SaveAsync(expense);
        await _unitOfWork.CommitAsync();
    }

    public async Task UpdateCardExpenseAsync(Guid id, CardExpenseRequest request)
    {
        var existingExpense = await _cardExpenseRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Card expense with id {id} not found.");

        _mapper.Map(request, existingExpense);

        _cardExpenseRepository.Update(existingExpense);
        await _unitOfWork.CommitAsync();
    }
}