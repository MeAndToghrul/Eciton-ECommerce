using Eciton.Application.DTOs.Product;
using Eciton.Application.ReadModels;
using Eciton.Application.ResponceObject;
namespace Eciton.Application.Abstractions;
public interface IProductService
{
    Task<Response> CreateAsync(CreateProductDTO product);

    Task<Response<ProductReadModel>> GetByIdAsync(string id);

    Task<Response<List<ProductReadModel>>> GetByNameAsync(string brand);

    Task<Response<List<ProductReadModel>>> GetAllAsync();
}
