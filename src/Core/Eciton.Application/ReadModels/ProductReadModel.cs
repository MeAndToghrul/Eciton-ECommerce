namespace Eciton.Application.ReadModels
{
    public class ProductReadModel
    {
        public string Id { get; set; } = null!;
        public string Brand { get; set; } = null!;
        public decimal Price { get; set; }
        public string CategoryName { get; set; } = null!;
        public int StockQuantity { get; set; }
        public string Model { get; set; } = null!;
        public string? ProductImage { get; set; }
    }
}
