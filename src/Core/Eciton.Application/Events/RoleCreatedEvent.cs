namespace Eciton.Application.Events;
public class RoleCreatedEvent
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public RoleCreatedEvent(string id, string name)
    {
        Id = id;
        Name = name;
    }
}
