using Application.DTOs;
using Application.Helpers.Pagination;
using Application.Interfaces;
using Application.Services.Base;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Base;
using AutoMapper;

namespace Application.Services;

public class ExpenseService : Service<ExpenseDto, Expense>, IExpenseService
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ExpenseService(
        IExpenseRepository repository,
        IUnitOfWork unitOfWork,
        IMapper mapper
    )
        : base(repository, unitOfWork, mapper)
    {
        _expenseRepository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    #region GET
    public async Task<IEnumerable<Expense>> GetAllExpensesAsync()
    {
        return await _expenseRepository.GetAllExpensesAsync();
    }

    public async Task<Expense?> GetExpenseByIdAsync(Guid id)
    {
        return await _expenseRepository.GetExpenseByIdAsync(id);
    }
    #endregion

    #region Add
    public async Task AddExpenseAsync(ExpenseDto expenseDto)
    {
        var expense = _mapper.Map<Expense>(expenseDto);

        await _expenseRepository.SaveAsync(expense);
        await _unitOfWork.CommitAsync();
    }
    #endregion

    #region Update
    public async Task UpdateExpenseAsync(ExpenseDto editExpense)
    {
        Guid? id = editExpense.Id
            ?? throw new ArgumentException("Expense ID cannot be null.", nameof(editExpense.Id));

        Expense expense = await _expenseRepository.GetByIdAsync(id.Value)
            ?? throw new KeyNotFoundException($"Expense with ID '{id}' not found.");

        expense = _mapper.Map(editExpense, expense);

        _expenseRepository.Update(expense);
        await _unitOfWork.CommitAsync();
    }
    #endregion

    #region Paginate
    public async Task<PagedResult<ExpenseDto>> GetPaginatedExpensesAsync(
        int pageNumber, int pageSize, string searchTerm = "")
    {
        var (expenses, totalItems) = await _expenseRepository
            .GetPaginatedAsync(pageNumber, pageSize, searchTerm);

        var expensesDTOs = _mapper.Map<IEnumerable<ExpenseDto>>(expenses);

        return new PagedResult<ExpenseDto>
        {
            Items = expensesDTOs,
            TotalItems = totalItems,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }
    #endregion
}