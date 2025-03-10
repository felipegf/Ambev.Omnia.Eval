using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

/// <summary>
/// Entity configuration for the SaleItem table.
/// </summary>
public class SaleItemConfiguration : IEntityTypeConfiguration<SaleItem>
{
    public void Configure(EntityTypeBuilder<SaleItem> builder)
    {
        builder.ToTable("SaleItems");

        builder.HasKey(si => si.Id);
        builder.Property(si => si.Id)
            .HasColumnType("uuid")
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(si => si.ProductId)
            .IsRequired();

        builder.Property(si => si.UnitPrice)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(si => si.Quantity)
            .IsRequired();

        builder.Property(si => si.Discount)
            .HasPrecision(18, 2)
            .HasDefaultValue(0);

        // Configuração da relação com Sale
        builder.HasOne<Sale>()
               .WithMany(s => s.SaleItems)
               .HasForeignKey("SaleId")
               .OnDelete(DeleteBehavior.Cascade);
    }
}
