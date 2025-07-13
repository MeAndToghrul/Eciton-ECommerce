using Eciton.Domain.Entities.Common;
namespace Eciton.Domain.Entities.Identity
{
    public class AppRole : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public IEnumerable<AppUser> Users { get; set; } = new List<AppUser>();
    }
}
