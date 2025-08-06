namespace Eciton.Application.Events;
public class CategoryCreatedEvent
{
    public string Id { get; set; }
    public string Name { get; set; } = null!;
    public string? CategoryImage { get; set; }

    public CategoryCreatedEvent(string id, string name, string? categoryimage)
    {
        Id = id;
        Name = name;
        CategoryImage = categoryimage;
    }
}
