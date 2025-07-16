using Application.DTOs.BankAccounts;
using Application.Interfaces;
using Application.Services.Base;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Base;

namespace Application.Services.Account;

public class BankAccountService : Service<BankAccountRequest, BankAccountResponse, BankAccount>, IBankAccountService
{
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public BankAccountService(
        IBankAccountRepository repository,
        IUnitOfWork unitOfWork,
        IMapper mapper
    )
        : base(repository, unitOfWork, mapper)
    {
        _bankAccountRepository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    #region Get
    public async Task<IEnumerable<BankAccountResponse>> GetAllBankAccountsAsync()
    {
        var bankAccounts = await _bankAccountRepository.GetAllBankAccountsAsync();
        return _mapper.Map<IEnumerable<BankAccountResponse>>(bankAccounts);
    }

    public async Task<BankAccountResponse> GetBankAccountByIdAsync(Guid id)
    {
        var bankAccount = await _bankAccountRepository.GetByIdAsync(id);
        return _mapper.Map<BankAccountResponse>(bankAccount);
    }
    #endregion

    #region Create
    public async Task CreateBankAccountAsync(BankAccountRequest request)
    {
        var bankAccount = _mapper.Map<BankAccount>(request);

        // Lógica de negócio aqui: definir CurrentBalance, etc.
        bankAccount.CurrentBalance = request.InitialBalance;

        await _bankAccountRepository.SaveAsync(bankAccount);
        await _unitOfWork.CommitAsync();
    }
    #endregion

    #region Update
    public async Task UpdateBankAccountAsync(Guid id, BankAccountRequest request)
    {
        var existingBankAccount = await _bankAccountRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Bank account with id {id} not found.");

        // Mapear DTO para a entidade existente, garantindo que o Id seja preservado
        _mapper.Map(request, existingBankAccount);

        // Lógica de negócio aqui, se necessário (ex: recalcular saldos)

        _bankAccountRepository.Update(existingBankAccount);
        await _unitOfWork.CommitAsync();
    }
    #endregion
}