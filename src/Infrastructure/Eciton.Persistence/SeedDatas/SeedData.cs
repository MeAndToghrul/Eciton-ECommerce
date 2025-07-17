using Eciton.Application.Abstractions;
using Eciton.Application.Events;
using Eciton.Application.Helpers;
using Eciton.Domain.Entities.Identity;
using Eciton.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Eciton.Persistence.SeedDatas;
public static class SeedData
{
    public static async Task SeedRolesAndAdminAsync(AppDbContext context, IEventBus eventBus)
    {
        // --- ROLLER ---
        var roles = new List<AppRole>
        {
            new AppRole { Id = Guid.NewGuid().ToString(), Name = "SuperAdmin"},
            new AppRole { Id = Guid.NewGuid().ToString(), Name = "Admin"},
            new AppRole { Id = Guid.NewGuid().ToString(), Name = "Guest"}
        };

        foreach (var role in roles)
        {
            if (!await context.AppRoles.AnyAsync(r => r.Name == role.Name))
            {
                await context.AppRoles.AddAsync(role);
                await eventBus.PublishAsync(new RoleCreatedEvent(
                    role.Id,
                    role.Name
                ));
            }
        }
        await context.SaveChangesAsync();


        var adminEmail = "admin@eciton.az";
        if (!await context.AppUsers.AnyAsync(u => u.Email == adminEmail))
        {
            var passwordService = new PasswordService();

            var adminRoleId = await context.AppRoles
                .Where(r => r.Name == "Admin")
                .Select(r => r.Id)
                .FirstOrDefaultAsync();

            var adminUser = new AppUser
            {
                Id = Guid.NewGuid().ToString(),
                FullName = "System Admin",
                Email = adminEmail,
                NormalizedEmail = adminEmail.ToUpper(),
                PasswordHash = passwordService.HashPassword("Admin123!"),
                RoleId = adminRoleId!,
                IsEmailConfirmed = true,
                LockoutEnabled = false,
                AccessFailedCount = 0,
                CreatedAt = DateTime.UtcNow
            };

            await context.AppUsers.AddAsync(adminUser);
            await context.SaveChangesAsync();

            await eventBus.PublishAsync(new UserRegisteredEvent(
                adminUser.Id,
                adminUser.FullName,
                adminUser.Email,
                adminUser.RoleId,
                adminUser.IsEmailConfirmed
            ));
        }
    }
}
