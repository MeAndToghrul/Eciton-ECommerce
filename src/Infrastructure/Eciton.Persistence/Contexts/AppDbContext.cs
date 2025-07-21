using Eciton.Domain.Entities.Entity;
using Eciton.Domain.Entities.Identity;
using Eciton.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Security.Claims;
namespace Eciton.Persistence.Contexts;
public class AppDbContext : DbContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    public DbSet<AuditLog> AuditLogs { get; set; }
    public DbSet<AppRole> AppRoles { get; set; }
    public DbSet<AppUser> AppUsers { get; set; }


    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var auditLogs = new List<AuditLog>();

        var entries = ChangeTracker.Entries()
            .Where(e =>
                e.State == EntityState.Added ||
                e.State == EntityState.Modified ||
                e.State == EntityState.Deleted);

        foreach (var entry in entries)
        {
            if (entry.Entity is AuditLog)
                continue;

            string entityName = entry.Metadata.ClrType.Name;
            string entityId = GetPrimaryKey(entry);
            string userId = GetCurrentUserId(entry);
            DateTime changeDate = DateTime.UtcNow;

            AuditAction actionType = entry.State switch
            {
                EntityState.Added => AuditAction.Create,
                EntityState.Deleted => HasSoftDelete(entry) ? AuditAction.SoftDelete : AuditAction.HardDelete,
                EntityState.Modified => AuditAction.Update,
                _ => throw new NotSupportedException()
            };

            foreach (var property in entry.Properties)
            {
                if (property.IsTemporary || property.Metadata.IsPrimaryKey())
                    continue;

                string? oldValue = null;
                string? newValue = null;
                bool shouldLog = false;

                if (entry.State == EntityState.Added)
                {
                    newValue = property.CurrentValue?.ToString();
                    shouldLog = true;
                }
                else if (entry.State == EntityState.Deleted)
                {
                    oldValue = property.OriginalValue?.ToString();
                    shouldLog = true;
                }
                else if (entry.State == EntityState.Modified && property.IsModified)
                {
                    oldValue = property.OriginalValue?.ToString();
                    newValue = property.CurrentValue?.ToString();
                    if (oldValue != newValue)
                        shouldLog = true;
                }

                if (property.Metadata.Name == "PasswordHash")
                {
                    oldValue = "***";
                    newValue = "***";
                    shouldLog = true;
                }

                if (!shouldLog) continue;

                auditLogs.Add(new AuditLog
                {
                    EntityName = entityName,
                    EntityId = entityId,
                    PropertyName = property.Metadata.Name,
                    OldValue = oldValue,
                    NewValue = newValue,
                    ChangeType = actionType,
                    ChangeDate = changeDate,
                    UserId = userId
                });
            }
        }

        if (auditLogs.Any())
            AuditLogs.AddRange(auditLogs);

        return await base.SaveChangesAsync(cancellationToken);
    }


    private string GetPrimaryKey(EntityEntry entry)
    {
        var keyProps = entry.Properties.Where(p => p.Metadata.IsPrimaryKey()).ToList();

        if (!keyProps.Any()) return "unknown";

        var keyValues = new Dictionary<string, object?>();

        foreach (var prop in keyProps)
        {
            var value = prop.CurrentValue ?? prop.OriginalValue;
            keyValues[prop.Metadata.Name] = value?.ToString() ?? "null";
        }

        return string.Join("-", keyValues.Values);
    }

    private bool HasSoftDelete(EntityEntry entry)
    {
        var isDeletedProp = entry.Properties
            .FirstOrDefault(p => p.Metadata.Name.Equals("IsDeleted", StringComparison.OrdinalIgnoreCase));

        if (isDeletedProp == null) return false;

        return entry.State == EntityState.Modified &&
               isDeletedProp.OriginalValue?.ToString() == "False" &&
               isDeletedProp.CurrentValue?.ToString() == "True";
    }

    private string GetCurrentUserId(EntityEntry entry)
    {
        if (entry.Entity is AppUser appUser && entry.State == EntityState.Added)
        {
            return appUser.Id.ToString();
        }

        var user = _httpContextAccessor.HttpContext?.User;

        if (user == null || !user.Identity?.IsAuthenticated == true)
            return "system";

        var userIdClaim = user.FindFirst("sub") ?? user.FindFirst("UserId") ?? user.FindFirst(ClaimTypes.NameIdentifier);
        return userIdClaim?.Value ?? "system";
    }







    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }


}
