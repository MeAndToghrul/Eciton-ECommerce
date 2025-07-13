namespace Eciton.Domain.Entities.Common;
public class BaseEntity
{
    public string Id { get; set; } = Guid.NewGuid().ToString();    
    public bool IsDeleted { get; set; } = false;
}
