using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infra.Mappings;

public class CreditCardMap : IEntityTypeConfiguration<CreditCard>
{
    public void Configure(EntityTypeBuilder<CreditCard> builder)
    {
        builder.ToTable("CreditCards");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Limit)
            .HasColumnType("decimal(18,2)");

        builder.Property(e => e.ClosingDay)
            .IsRequired();

        builder.Property(e => e.DueDay)
            .IsRequired();

        builder.HasOne(e => e.BankAccount)
            .WithMany()
            .HasForeignKey(e => e.BankAccountId)
            .IsRequired();

        builder.HasMany(c => c.Invoices)
            .WithOne(i => i.CreditCard)
            .HasForeignKey(i => i.CreditCardId);

        builder.HasMany(c => c.CardExpenses)
            .WithOne(i => i.CreditCard)
            .HasForeignKey(i => i.CreditCardId);
            
        builder.Property(e => e.CreatedAt)
            .IsRequired();

        builder.Property(e => e.UpdatedAt)
            .IsRequired(false);
    }
}