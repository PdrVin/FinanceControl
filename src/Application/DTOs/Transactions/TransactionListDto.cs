namespace Application.DTOs.Transactions;

public class TransactionListDto
{
    public IEnumerable<TransactionDto> Transactions { get; set; }
    public decimal CurrentBalance { get; set; }
    public decimal TotalIncomes { get; set; }
    public decimal TotalExpenses { get; set; }
    public decimal MonthlyBalance { get; set; }
    public int TotalPages { get; set; }
    public int CurrentPage { get; set; }
    public string MonthName { get; set; }
    public int Year { get; set; }
}