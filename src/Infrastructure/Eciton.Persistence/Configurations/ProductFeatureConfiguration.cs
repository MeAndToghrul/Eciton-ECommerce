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
    public class ProductFeatureConfiguration : IEntityTypeConfiguration<ProductFeature>
    {
        public void Configure(EntityTypeBuilder<ProductFeature> builder)
        {
            builder.ToTable("ProductFeatures");

            builder.HasKey(pf => pf.Id);

            builder.Property(pf => pf.Id)
                .IsRequired();


            builder.Property(pf => pf.ProductId)
                .IsRequired();
                

            builder.Property(pf => pf.Key)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("varchar(100)");

            builder.Property(pf => pf.Value)
                .IsRequired()
                .HasMaxLength(250)
                .HasColumnType("varchar(250)");

            builder.Property(pf => pf.DataType)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnType("varchar(50)");

            builder.Property(pf => pf.CreatedAt)
                .IsRequired()
                .HasColumnType("timestamp with time zone");

            builder.Property(pf => pf.UpdatedAt)
                .HasColumnType("timestamp with time zone");

            builder.Property(pf => pf.IsDeleted)
                .IsRequired()
                .HasColumnType("boolean");

            builder.HasOne(pf => pf.Product)
                .WithMany(p => p.Features)
                .HasForeignKey(pf => pf.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
