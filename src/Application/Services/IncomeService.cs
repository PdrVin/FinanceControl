using Application.DTOs;
using Application.Interfaces;
using Application.Services.Base;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Base;
using AutoMapper;
using Application.Helpers.Pagination;

namespace Application.Services;

public class IncomeService : Service<IncomeDto, Income>, IIncomeService
{
    private readonly IIncomeRepository _incomeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public IncomeService(
        IIncomeRepository repository,
        IUnitOfWork unitOfWork,
        IMapper mapper
    )
        : base(repository, unitOfWork, mapper)
    {
        _incomeRepository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    #region GET
    public async Task<IEnumerable<Income>> GetAllIncomesAsync()
    {
        return await _incomeRepository.GetAllIncomesAsync();
    }

    public async Task<Income?> GetIncomeByIdAsync(Guid id)
    {
        return await _incomeRepository.GetIncomeByIdAsync(id);
    }
    #endregion

    #region Add
    public async Task AddIncomeAsync(IncomeDto incomeDto)
    {
        var income = _mapper.Map<Income>(incomeDto);

        await _incomeRepository.SaveAsync(income);
        await _unitOfWork.CommitAsync();
    }
    #endregion

    #region Update
    public async Task UpdateIncomeAsync(IncomeDto editIncome)
    {
        Guid? id = editIncome.Id
            ?? throw new ArgumentException("Income ID cannot be null.", nameof(editIncome.Id));

        Income income = await _incomeRepository.GetByIdAsync(id.Value)
            ?? throw new KeyNotFoundException($"Income with ID '{id}' not found.");

        income = _mapper.Map(editIncome, income);

        _incomeRepository.Update(income);
        await _unitOfWork.CommitAsync();
    }
    #endregion

    #region Paginate
    public async Task<PagedResult<IncomeDto>> GetPaginatedIncomesAsync(
        int pageNumber, int pageSize, string searchTerm = "")
    {
        var (incomes, totalItems) = await _incomeRepository
            .GetPaginatedAsync(pageNumber, pageSize, searchTerm);

        var incomeDTOs = _mapper.Map<IEnumerable<IncomeDto>>(incomes);

        return new PagedResult<IncomeDto>
        {
            Items = incomeDTOs,
            TotalItems = totalItems,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }
    #endregion
}
