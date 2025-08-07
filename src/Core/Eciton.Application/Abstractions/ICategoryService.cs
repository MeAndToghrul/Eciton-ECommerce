using Eciton.Application.DTOs.Category;
using Eciton.Application.ReadModels;
using Eciton.Application.ResponceObject;
namespace Eciton.Application.Abstractions
{
    public interface ICategoryService
    {
        Task<Response>  CreateAsync(CreateCategoryDTO categoryDTO);
        Task<Response<CategoryReadModel>>  GetByIdAsync(string id);
        Task<Response<CategoryReadModel>>  GetByNameAsync(string name);
        Task<Response<List<CategoryReadModel>>> GetAllAsync();
        Task<bool> ExistsAsync(string idOrName);
    }
}
