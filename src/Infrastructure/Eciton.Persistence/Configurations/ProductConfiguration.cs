using Eciton.Domain.Entities.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eciton.Persistence.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");


            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(150)
                .HasColumnType("varchar(150)");

            builder.Property(p => p.Price)
                .IsRequired()
                .HasColumnType("numeric(18,2)"); 

            builder.Property(p => p.Brand)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("varchar(100)");

            builder.Property(p => p.CategoryId)
                .IsRequired();
                

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
}
