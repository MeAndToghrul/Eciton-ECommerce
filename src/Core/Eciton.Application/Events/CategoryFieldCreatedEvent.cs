namespace Eciton.Application.Events;
public class CategoryFieldCreatedEvent
{
    public string Id { get; set; }
    public string CategoryId { get; set; } = null!;
    public string FieldName { get; set; } = null!;
    public string DataType { get; set; } = null!;

    public CategoryFieldCreatedEvent(string id, string categoryId, string fieldName, string dataType)
    {
        Id = id;
        CategoryId = categoryId;
        FieldName = fieldName;
        DataType = dataType;
    }
}
