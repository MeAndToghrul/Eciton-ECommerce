using Eciton.Domain.Entities.Entity;
using Eciton.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;

namespace Eciton.Persistence.Contexts;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<AuditLog> AuditLogs { get; set; }
    public DbSet<AppRole> AppRoles { get; set; }
    public DbSet<AppUser> AppUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }


}
