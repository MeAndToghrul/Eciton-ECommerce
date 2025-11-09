using Microsoft.AspNetCore.Http;

namespace Eciton.Application.DTOs.Product;
public class CreateProductDTO
{
    public string Brand { get; set; }
    public decimal Price { get; set; }
    public string Model { get; set; }    
    public int StockQuantity { get; set; }
    public IFormFile ProductImage { get; set; }
    public string CategoryId { get; set; }
}
