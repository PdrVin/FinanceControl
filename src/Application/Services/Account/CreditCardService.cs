using Application.DTOs.CreditCards;
using Application.Interfaces;
using Application.Services.Base;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Base;

namespace Application.Services.Account;

public class CreditCardService : Service<CreditCardRequest, CreditCardResponse, CreditCard>, ICreditCardService
{
    private readonly ICreditCardRepository _creditCardRepository;
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreditCardService(
        ICreditCardRepository repository,
        IBankAccountRepository bankAccountRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper
    )
        : base(repository, unitOfWork, mapper)
    {
        _creditCardRepository = repository;
        _bankAccountRepository = bankAccountRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CreditCardResponse>> GetAllCreditCardsAsync()
    {
        var creditCards = await _creditCardRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<CreditCardResponse>>(creditCards);
    }

    public async Task<CreditCardResponse> GetCreditCardByIdAsync(Guid id)
    {
        var creditCard = await _creditCardRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Credit card with id {id} not found.");

        return _mapper.Map<CreditCardResponse>(creditCard);
    }

    public async Task CreateCreditCardAsync(CreditCardRequest request)
    {
        // Lógica de negócio: Verificar se a conta bancária existe
        _ = await _bankAccountRepository.GetByIdAsync(request.BankAccountId)
            ?? throw new KeyNotFoundException($"Bank account with id {request.BankAccountId} not found.");

        var creditCard = _mapper.Map<CreditCard>(request);

        await _creditCardRepository.SaveAsync(creditCard);
        await _unitOfWork.CommitAsync();
    }

    public async Task UpdateCreditCardAsync(Guid id, CreditCardRequest request)
    {
        var existingCreditCard = await _creditCardRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Credit card with id {id} not found.");

        // Lógica de negócio: Verificar se a nova conta bancária existe, se o ID for alterado
        if (request.BankAccountId != existingCreditCard.BankAccountId)
        {
            var newBankAccount = await _bankAccountRepository.GetByIdAsync(request.BankAccountId)
                ?? throw new KeyNotFoundException($"New bank account with id {request.BankAccountId} not found.");
            
            existingCreditCard.BankAccountId = newBankAccount.Id;
        }

        _mapper.Map(request, existingCreditCard);

        _creditCardRepository.Update(existingCreditCard);
        await _unitOfWork.CommitAsync();
    }
}