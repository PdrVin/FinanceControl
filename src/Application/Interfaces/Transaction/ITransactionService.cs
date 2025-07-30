using Application.DTOs.Transactions;

namespace Application.Interfaces.Transaction;

public interface ITransactionService
{
    Task<TransactionListDto> GetTransactionsByMonthAndYearAsync(
        int year, int month, int pageNumber, int pageSize);
}