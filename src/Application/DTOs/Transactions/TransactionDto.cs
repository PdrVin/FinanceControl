namespace Application.DTOs.Transactions;

public class TransactionDto
{
    public Guid Id { get; set; }
    public string Type { get; set; }
    public bool Status { get; set; }
    public DateTime Date { get; set; }
    public string? Description { get; set; }
    public string CategoryName { get; set; }
    public string AccountName { get; set; }
    public decimal Amount { get; set; }
    public Guid? RelatedAccountId { get; set; }
    public Guid? RelatedCardId { get; set; }
}