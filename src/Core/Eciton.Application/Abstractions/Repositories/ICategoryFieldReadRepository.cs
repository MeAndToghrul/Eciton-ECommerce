using Eciton.Application.ReadModels;
using Eciton.Application.ResponceObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eciton.Application.Abstractions.Repositories
{
    public interface ICategoryFieldReadRepository
    {
        Task<Response<List<CategoryFieldReadModel>>> GetAllAsync();
        Task<Response<CategoryFieldReadModel>> GetByIdAsync(string id);
        Task<Response<List<CategoryFieldReadModel>>> GetByCategoryIdAsync(string categoryId);
    }
}
