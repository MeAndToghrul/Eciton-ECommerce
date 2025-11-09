using Eciton.Application.DTOs.CategoryField;
using Eciton.Application.ReadModels;
using Eciton.Application.ResponceObject;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eciton.Application.Abstractions
{
    public interface ICategoryFieldService
    {
        Task<Response> CreateAsync(CreateCategoryFieldDTO categoryField);

        Task<Response<CategoryFieldReadModel>> GetByIdAsync(string id);

        Task<Response<List<CategoryFieldReadModel>>> GetByCategoryIdAsync(string categoryId);

        Task<Response<List<CategoryFieldReadModel>>> GetAllAsync();
    }
}
