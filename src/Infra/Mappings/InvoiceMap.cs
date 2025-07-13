using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infra.Mappings;

public class InvoiceMap : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.ToTable("Invoices");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.InvoiceName)
            .IsRequired();
        
        builder.Property(e => e.ReferenceMonth)
            .IsRequired();

        builder.Property(e => e.ReferenceYear)
            .IsRequired();

        builder.Property(e => e.TotalAmount)
            .HasColumnType("decimal(18,2)");

        builder.Property(e => e.ClosingDate)
            .IsRequired();

        builder.Property(e => e.DueDate)
            .IsRequired();

        builder.Property(e => e.IsPaid)
            .IsRequired();

        builder.HasOne(e => e.CreditCard)
            .WithMany(c => c.Invoices)
            .HasForeignKey(e => e.CreditCardId)
            .IsRequired();

        builder.HasMany(e => e.CardExpenses)
            .WithOne(ce => ce.Invoice)
            .HasForeignKey(ce => ce.InvoiceId)
            .IsRequired(false);

        builder.Property(e => e.CreatedAt)
            .IsRequired();

        builder.Property(e => e.UpdatedAt)
            .IsRequired(false);
    }
}