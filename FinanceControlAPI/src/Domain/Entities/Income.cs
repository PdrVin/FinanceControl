using Domain.Enums;

namespace Domain.Entities;

    public class Income
    {
        public Guid Id { get; set; }
        public Status Status { get; set; }
        public required PayType Type { get; set; }
        public required DateTime Date { get; set; }
        public required string Description { get; set; }
        public required Category Category { get; set; }
        public required Account Account { get; set; }
        public decimal Amount { get; set; }
    }
