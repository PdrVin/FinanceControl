using Application.DTOs.Transactions;
using Domain.Interfaces;
using Application.Interfaces.Transaction;

namespace Application.Services.Transaction;

public class TransactionService : ITransactionService
{
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly IIncomeRepository _incomeRepository;
    private readonly IExpenseRepository _expenseRepository;
    private readonly ITransferRepository _transferRepository;
    private readonly ICardExpenseRepository _cardExpenseRepository;

    public TransactionService(
        IBankAccountRepository bankAccountRepository,
        IIncomeRepository incomeRepository,
        IExpenseRepository expenseRepository,
        ITransferRepository transferRepository,
        ICardExpenseRepository cardExpenseRepository)
    {
        _bankAccountRepository = bankAccountRepository;
        _incomeRepository = incomeRepository;
        _expenseRepository = expenseRepository;
        _transferRepository = transferRepository;
        _cardExpenseRepository = cardExpenseRepository;
    }

    public async Task<TransactionListDto> GetTransactionsByMonthAndYearAsync(int year, int month, int pageNumber, int pageSize)
    {
        var startDate = new DateTime(year, month, 1);
        var endDate = startDate.AddMonths(1).AddDays(-1);

        var incomes = await _incomeRepository.GetIncomesByPeriodAsync(startDate, endDate);
        var expenses = await _expenseRepository.GetExpensesByPeriodAsync(startDate, endDate);
        var transfers = await _transferRepository.GetTransfersByPeriodAsync(startDate, endDate);
        var cardExpenses = await _cardExpenseRepository.GetCardExpensesByPeriodAsync(startDate, endDate);

        var allTransactions = new List<TransactionDto>();

        allTransactions.AddRange(incomes.Select(r => new TransactionDto
        {
            Id = r.Id,
            Type = "Income",
            Status = r.IsReceived,
            Date = r.Date,
            Description = r.Description,
            CategoryName = r.Category.ToString(),
            AccountName = r.BankAccount!.Name,
            Amount = r.Amount,
            RelatedAccountId = r.BankAccountId,
        }));

        allTransactions.AddRange(expenses.Select(e => new TransactionDto
        {
            Id = e.Id,
            Type = "Expense",
            Status = e.IsPaid,
            Date = e.Date,
            Description = e.Description,
            CategoryName = e.Category.ToString(),
            AccountName = e.BankAccount!.Name,
            Amount = -e.Amount,
            RelatedAccountId = e.BankAccountId
        }));

        allTransactions.AddRange(transfers.Select(t => new TransactionDto
        {
            Id = t.Id,
            Type = "Transfer",
            Status = true,
            Date = t.Date,
            Description = null,
            CategoryName = "Transferência",
            AccountName = $"{t.SourceAccount!.Name} -> {t.DestinationAccount!.Name}",
            Amount = 0,
            RelatedAccountId = t.SourceAccountId
        }));

        allTransactions.AddRange(cardExpenses.Select(cce => new TransactionDto
        {
            Id = cce.Id,
            Type = "CardExpense",
            Status = false,
            Date = cce.Invoice.DueDate,
            Description = cce.Description,
            CategoryName = cce.Category.ToString(),
            AccountName = cce.CreditCard!.Name,
            Amount = -cce.Amount,
            RelatedCardId = cce.CreditCardId
        }));

        allTransactions = allTransactions.OrderByDescending(t => t.Date).ToList();

        var totalIncomes = allTransactions
            .Where(t => t.Type == "Income").Sum(t => t.Amount);
        var totalExpenses = allTransactions
            .Where(t => t.Type == "Expense" || t.Type == "CardExpense").Sum(t => t.Amount);
        var monthlyBalance = totalIncomes + totalExpenses;

        var allBankAccounts = await _bankAccountRepository.GetAllBankAccountsAsync();
        var currentOverallBalance = allBankAccounts.Sum(ba => ba.CurrentBalance);

        var totalItems = allTransactions.Count;
        var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
        var paginatedTransactions = allTransactions
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        // Novos atributos: MonthName (em português) e Year
        var monthName = System.Globalization.CultureInfo.GetCultureInfo("pt-BR").DateTimeFormat.GetMonthName(month);
        
        return new TransactionListDto
        {
            Transactions = paginatedTransactions,
            CurrentBalance = currentOverallBalance,
            TotalIncomes = totalIncomes,
            TotalExpenses = Math.Abs(totalExpenses),
            MonthlyBalance = monthlyBalance,
            TotalPages = totalPages,
            CurrentPage = pageNumber,
            MonthName = monthName,
            Year = year
        };
    }
}