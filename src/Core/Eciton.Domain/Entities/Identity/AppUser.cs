using Eciton.Domain.Entities.Common;
namespace Eciton.Domain.Entities.Identity;
public class AppUser : BaseEntity
{
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string NormalizedEmail { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string RoleId { get; set; } = null!;
    public AppRole Role { get; set; } = null!;
    public bool IsEmailConfirmed { get; set; } = false;
    public bool LockoutEnabled { get; set; } = true;
    public DateTime? LastFailedAttempt { get; set; }
    public DateTime? LockoutEnd { get; set; }
    public int AccessFailedCount { get; set; } = 0;        
    public int MaxFailedAccessAttempts { get; set; } = 5; 
}
