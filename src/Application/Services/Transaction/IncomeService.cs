using Application.DTOs.Incomes;
using Application.Helpers;
using Application.Interfaces;
using Application.Services.Base;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Interfaces.Base;
using AutoMapper;

namespace Application.Services.Transaction;

public class IncomeService : Service<IncomeRequest, IncomeResponse,Income>, IIncomeService
{
    private readonly IIncomeRepository _incomeRepository;
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public IncomeService(
        IIncomeRepository repository,
        IBankAccountRepository bankAccountRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper
    )
        : base(repository, unitOfWork, mapper)
    {
        _incomeRepository = repository;
        _bankAccountRepository = bankAccountRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    #region Get
    public async Task<IEnumerable<IncomeResponse>> GetAllIncomesAsync()
    {
        var incomes = await _incomeRepository.GetAllIncomesAsync();
        return _mapper.Map<IEnumerable<IncomeResponse>>(incomes);
    }
    
    public async Task<IncomeResponse> GetIncomeByIdAsync(Guid id)
    {
        var income = await _incomeRepository.GetIncomeByIdAsync(id);
        return _mapper.Map<IncomeResponse>(income);
    }

    public async Task<IEnumerable<IncomeResponse>> GetIncomesByBankAccountIdAsync(Guid bankAccountId)
    {
        var incomes = await _incomeRepository.GetIncomesByBankAccountIdAsync(bankAccountId);
        return _mapper.Map<IEnumerable<IncomeResponse>>(incomes);
    }

    public async Task<IEnumerable<IncomeResponse>> GetIncomesByCategoryAsync(IncomeCategory category)
    {
        var incomes = await _incomeRepository.GetIncomesByCategoryAsync(category);
        return _mapper.Map<IEnumerable<IncomeResponse>>(incomes);
    }
    #endregion

    #region Create
    public async Task CreateIncomeAsync(IncomeRequest request)
    {
        var bankAccount = await _bankAccountRepository.GetByIdAsync(request.BankAccountId)
            ?? throw new KeyNotFoundException($"Bank account with id {request.BankAccountId} not found.");

        bankAccount.CurrentBalance += request.Amount;
        _bankAccountRepository.Update(bankAccount);

        var income = _mapper.Map<Income>(request);

        await _incomeRepository.SaveAsync(income);
        await _unitOfWork.CommitAsync();
    }
    #endregion

    #region Update
    public async Task UpdateIncomeAsync(Guid id, IncomeRequest request)
    {
        var existingIncome = await _incomeRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Income with id {id} not found.");

        // Lógica de negócio: Reverter o saldo antigo e aplicar o novo
        var bankAccount = await _bankAccountRepository.GetByIdAsync(existingIncome.BankAccountId)
            ?? throw new KeyNotFoundException($"Bank account with id {existingIncome.BankAccountId} not found.");

        bankAccount.CurrentBalance -= existingIncome.Amount;
        bankAccount.CurrentBalance += request.Amount;
        _bankAccountRepository.Update(bankAccount);

        _mapper.Map(request, existingIncome);
        _incomeRepository.Update(existingIncome);
        await _unitOfWork.CommitAsync();
    }
    #endregion

    #region Delete
    public async Task DeleteIncomeAsync(Guid id)
    {
        // Lógica de negócio: Reverter o saldo da conta antes de deletar
        var existingIncome = await _incomeRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Income with id {id} not found.");

        var bankAccount = await _bankAccountRepository.GetByIdAsync(existingIncome.BankAccountId)
            ?? throw new KeyNotFoundException($"Bank account with id {existingIncome.BankAccountId} not found.");

        bankAccount.CurrentBalance -= existingIncome.Amount;
        _bankAccountRepository.Update(bankAccount);

        _incomeRepository.Delete(id);
        await _unitOfWork.CommitAsync();
    }
    #endregion

    #region Paginate
    public async Task<PagedResult<IncomeResponse>> GetPaginatedIncomesAsync(
        int pageNumber, int pageSize, string searchTerm = "")
    {
        var (incomes, totalItems) = await _incomeRepository
            .GetPaginatedAsync(pageNumber, pageSize, searchTerm);

        var incomeDTOs = _mapper.Map<IEnumerable<IncomeResponse>>(incomes);

        return new PagedResult<IncomeResponse>
        {
            Items = incomeDTOs,
            TotalItems = totalItems,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }
    #endregion
}