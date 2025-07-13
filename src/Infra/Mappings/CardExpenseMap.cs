using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infra.Mappings;

public class CardExpenseMap : IEntityTypeConfiguration<CardExpense>
{
    public void Configure(EntityTypeBuilder<CardExpense> builder)
    {
        builder.ToTable("CardExpenses");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Description)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(e => e.Amount)
            .HasColumnType("decimal(18,2)");

        builder.Property(e => e.Date)
            .IsRequired();

        builder.Property(e => e.Category)
            .IsRequired()
            .HasConversion<string>();

        builder.HasOne(e => e.CreditCard)
            .WithMany(c => c.CardExpenses)
            .HasForeignKey(e => e.CreditCardId)
            .IsRequired();

        builder.HasOne(e => e.Invoice)
            .WithMany(i => i.CardExpenses)
            .HasForeignKey(e => e.InvoiceId)
            .IsRequired(false);
    }
}
