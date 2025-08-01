using Application.DTOs.Expenses;
using Application.Helpers;
using Application.Interfaces;
using Application.Services.Base;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Interfaces.Base;
using AutoMapper;

namespace Application.Services.Transaction;

public class ExpenseService : Service<ExpenseRequest, ExpenseResponse, Expense>, IExpenseService
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ExpenseService(
        IExpenseRepository repository,
        IBankAccountRepository bankAccountRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper
    )
        : base(repository, unitOfWork, mapper)
    {
        _expenseRepository = repository;
        _bankAccountRepository = bankAccountRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    #region GET
    public async Task<IEnumerable<ExpenseResponse>> GetAllExpensesAsync()
    {
        var expenses = await _expenseRepository.GetAllExpensesAsync();
        return _mapper.Map<IEnumerable<ExpenseResponse>>(expenses);
    }

    public async Task<ExpenseResponse> GetExpenseByIdAsync(Guid id)
    {
        var expense = await _expenseRepository.GetExpenseByIdAsync(id);
        return _mapper.Map<ExpenseResponse>(expense);
    }

    public async Task<IEnumerable<ExpenseResponse>> GetExpensesByBankAccountIdAsync(Guid bankAccountId)
    {
        var expenses = await _expenseRepository.GetExpensesByBankAccountIdAsync(bankAccountId);
        return _mapper.Map<IEnumerable<ExpenseResponse>>(expenses);
    }

    public async Task<IEnumerable<ExpenseResponse>> GetExpensesByCategoryAsync(ExpenseCategory category)
    {
        var expenses = await _expenseRepository.GetExpensesByCategoryAsync(category);
        return _mapper.Map<IEnumerable<ExpenseResponse>>(expenses);
    }
    #endregion

    #region Create
    public async Task CreateExpenseAsync(ExpenseRequest request)
    {
        // Lógica de negócio: Validar a existência da conta e atualizar o saldo
        var bankAccount = await _bankAccountRepository.GetByIdAsync(request.BankAccountId)
            ?? throw new KeyNotFoundException($"Bank account with id {request.BankAccountId} not found.");

        var expense = _mapper.Map<Expense>(request);

        bankAccount.CurrentBalance -= request.Amount;
        _bankAccountRepository.Update(bankAccount);

        await _expenseRepository.SaveAsync(expense);
        await _unitOfWork.CommitAsync();
    }
    #endregion

    #region Update
    public async Task UpdateExpenseAsync(Guid id, ExpenseRequest request)
    {
        var existingExpense = await _expenseRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Expense with id {id} not found.");

        // Lógica de negócio: Reverter o saldo antigo e aplicar o novo
        var bankAccount = await _bankAccountRepository.GetByIdAsync(existingExpense.BankAccountId)
            ?? throw new KeyNotFoundException($"Bank account with id {existingExpense.BankAccountId} not found.");

        bankAccount.CurrentBalance += existingExpense.Amount;
        bankAccount.CurrentBalance -= request.Amount;
        _bankAccountRepository.Update(bankAccount);

        _mapper.Map(request, existingExpense);
        _expenseRepository.Update(existingExpense);
        await _unitOfWork.CommitAsync();
    }
    #endregion

    #region Delete
    public async Task DeleteExpenseAsync(Guid id)
    {
        // Lógica de negócio: Reverter o saldo da conta antes de deletar
        var expense = await _expenseRepository.GetExpenseByIdAsync(id)
            ?? throw new KeyNotFoundException($"Expense with id {id} not found.");

        expense.BankAccount.CurrentBalance += expense.Amount;
        _bankAccountRepository.Update(expense.BankAccount);

        _expenseRepository.Delete(id);
        await _unitOfWork.CommitAsync();
    }
    #endregion

    #region Paginate
    public async Task<PagedResult<ExpenseResponse>> GetPaginatedExpensesAsync(
        int pageNumber, int pageSize, string searchTerm = "")
    {
        var (expenses, totalItems) = await _expenseRepository
            .GetPaginatedAsync(pageNumber, pageSize, searchTerm);

        var expensesDTOs = _mapper.Map<IEnumerable<ExpenseResponse>>(expenses);

        return new PagedResult<ExpenseResponse>
        {
            Items = expensesDTOs,
            TotalItems = totalItems,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }
    #endregion
}