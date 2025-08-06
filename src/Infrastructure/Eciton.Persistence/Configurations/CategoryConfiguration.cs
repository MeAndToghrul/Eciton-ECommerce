using Eciton.Domain.Entities.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Eciton.Persistence.Configurations;
public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnType("varchar(100)");

        builder.Property(c => c.CreatedAt)
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder.Property(c => c.UpdatedAt)
            .HasColumnType("timestamp with time zone");

        builder.Property(p => p.IsDeleted)
                .IsRequired()
                .HasColumnType("boolean");

        builder.HasMany(c => c.Fields)
            .WithOne()
            .HasForeignKey("CategoryId") 
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.Products)
            .WithOne()
            .HasForeignKey("CategoryId") 
            .OnDelete(DeleteBehavior.Cascade);
    }
}
