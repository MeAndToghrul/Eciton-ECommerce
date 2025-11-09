namespace Eciton.Application.Events;
public class ProductCreatedEvent
{
    public string Id { get; set; }
    public string Brand { get; set; } = null!;
    public decimal Price { get; set; }
    public string CategoryName { get; set; } = null!;
    public int StockQuantity { get; set; }
    public string Model { get; set; } = null!;
    public string? ProductImage { get; set; }

    public ProductCreatedEvent(string id, string brand, decimal price, string categoryName,int stockQuantity, string model, string? productImage = null)
    {
        Id = id;
        Brand = brand;
        Price = price;
        CategoryName = categoryName;
        StockQuantity = stockQuantity;
        Model = model;
        ProductImage = productImage;
    }
}
