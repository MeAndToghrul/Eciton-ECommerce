using Eciton.Domain.Entities.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eciton.Persistence.Configurations;
public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.ToTable("AuditLogs");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.EntityName)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnType("varchar(100)");

        builder.Property(x => x.EntityId)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnType("varchar(50)");

        builder.Property(x => x.PropertyName)
            .HasMaxLength(100)
            .HasColumnType("varchar(100)");

        builder.Property(x => x.OldValue)
            .HasColumnType("text");

        builder.Property(x => x.NewValue)
            .HasColumnType("text");

        builder.Property(x => x.ChangeType)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20)
            .HasColumnType("varchar(20)");

        builder.Property(x => x.ChangeDate)
            .IsRequired()
            .HasColumnType("timestamp with time zone");

        builder.Property(x => x.UserId)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnType("varchar(50)");
    }
}
