using Eciton.Domain.Entities.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Eciton.Persistence.Configurations;
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Brand)
            .IsRequired()
            .HasMaxLength(150)
            .HasColumnType("varchar(150)");

        builder.Property(p => p.Price)
            .IsRequired()
            .HasColumnType("numeric(18,2)");

        builder.Property(p => p.Model)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnType("varchar(100)");

        builder.Property(p => p.CategoryId)
            .IsRequired();

        builder.Property(p => p.IsStock)
            .IsRequired()
            .HasColumnType("boolean");

        builder.Property(p => p.StockQuantity)
            .IsRequired()
            .HasColumnType("int");

        builder.Property(p => p.ProductImage)
            .HasMaxLength(250)
            .HasColumnType("varchar(250)");

        builder.Property(p => p.DiscountPercentage)
            .HasColumnType("numeric(5,2)"); 

        builder.Property(p => p.DiscountStartDate)
            .HasColumnType("timestamp with time zone");

        builder.Property(p => p.DiscountEndDate)
            .HasColumnType("timestamp with time zone");

        builder.Property(p => p.CreatedAt)
            .IsRequired()
            .HasColumnType("timestamp with time zone");

        builder.Property(p => p.UpdatedAt)
            .HasColumnType("timestamp with time zone");

        builder.Property(p => p.IsDeleted)
            .IsRequired()
            .HasColumnType("boolean");

        builder.HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Features)
            .WithOne(pf => pf.Product)
            .HasForeignKey(pf => pf.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
