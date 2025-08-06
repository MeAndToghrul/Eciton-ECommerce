using Eciton.Domain.Entities.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eciton.Persistence.Configurations;

public class CategoryFieldConfiguration : IEntityTypeConfiguration<CategoryField>
{
    public void Configure(EntityTypeBuilder<CategoryField> builder)
    {
        builder.ToTable("CategoryFields");

        builder.HasKey(cf => cf.Id);

        builder.Property(cf => cf.FieldName)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnType("varchar(100)");

        builder.Property(cf => cf.DataType)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnType("varchar(50)");

        builder.Property(cf => cf.CreatedAt)
            .IsRequired()
            .HasColumnType("timestamp with time zone");

        builder.Property(cf => cf.UpdatedAt)
            .HasColumnType("timestamp with time zone");

        builder.Property(p => p.IsDeleted)
                .IsRequired()
                .HasColumnType("boolean");

        builder.HasOne(cf => cf.Category)
            .WithMany(c => c.Fields)
            .HasForeignKey(cf => cf.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
