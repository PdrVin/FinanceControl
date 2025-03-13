using Domain.Entities.Base;
using Domain.Enums;

namespace Domain.Entities;

public class Income : EntityBase
{
    public Status Status { get; set; }
    public PayType PayType { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; }
    public IncomeCategory Category { get; set; }
    public Account Account { get; set; }
    public decimal Amount { get; set; }

    protected Income() { }

    public Income(
        string description,
        IncomeCategory category,
        decimal amount,
        DateTime date,
        Status status = Status.Pendente,
        PayType type = PayType.Pix,
        Account account = Account.PicPay
    )
    {
        if (date == default)
            throw new ArgumentException("A data fornecida é inválida.", nameof(date));
        
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("A descrição não pode ser nula ou vazia.", nameof(description));

        if (!Enum.IsDefined(typeof(IncomeCategory), category))
            throw new ArgumentException("Categoria inválida.", nameof(category));

        if (amount <= 0)
            throw new ArgumentException("O valor deve ser maior que zero.", nameof(amount));

        Status = status;
        PayType = type;
        Date = date;
        Description = description;
        Category = category;
        Account = account;
        Amount = amount;
    }
}
