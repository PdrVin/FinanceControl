using Domain.Entities.Base;
using Domain.Enums;

namespace Domain.Entities;

    public class Invoice : EntityBase
    {
        public required string CardName { get; set; }
        public Account Account { get; set; }
        public DateTime ClosingDate { get; set; }
        public DateTime DueDate { get; set; }
        public decimal TotalAmount { get; private set; }
        public ICollection<Expense>? Expenses { get; private set; } = new List<Expense>();

        public Invoice()
        {
            Expenses = new List<Expense>();
        }

        public Invoice(
            string cardName,
            Account account,
            DateTime closingDate,
            DateTime dueDate,
            ICollection<Expense>? expenses = null
        )
        {
            if (string.IsNullOrWhiteSpace(cardName))
                throw new ArgumentException("O nome do cartão não pode ser vazio.", nameof(cardName));

            if (closingDate >= dueDate)
                throw new ArgumentException("A data de fechamento deve ser anterior à data de vencimento.", nameof(closingDate));

            CardName = cardName;
            Account = account;
            ClosingDate = closingDate;
            DueDate = dueDate;
            Expenses = expenses ?? new List<Expense>();;
            CalculateTotalAmount();
        }

        public void AddExpense(Expense expense)
        {
            if (expense == null)
                throw new ArgumentNullException(nameof(expense), "A despesa não pode ser nula.");

            if (expense.InvoiceId != null && expense.InvoiceId != Id)
                throw new ArgumentException("A despesa pertence a outra fatura.", nameof(expense.InvoiceId));

            Expenses?.Add(expense);
            CalculateTotalAmount();
        }

        private void CalculateTotalAmount()
        {
            TotalAmount = Expenses?.Sum(ex => ex.Amount) ?? 0;
        }
    }