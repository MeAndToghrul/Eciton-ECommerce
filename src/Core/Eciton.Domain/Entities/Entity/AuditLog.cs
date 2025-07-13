using Eciton.Domain.Enums;

namespace Eciton.Domain.Entities.Entity
{
    public class AuditLog
    {
        public string Id { get; set; }
        public required string EntityName { get; set; }
        public required string EntityId { get; set; }
        public string? PropertyName { get; set; }
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
        public required AuditAction ChangeType { get; set; }
        public required DateTime ChangeDate { get; set; }
        /*public required string UserId { get; set; }*/
    }
}
