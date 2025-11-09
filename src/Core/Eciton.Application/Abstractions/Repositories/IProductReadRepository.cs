using Eciton.Application.ReadModels;
using Eciton.Application.ResponceObject;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eciton.Application.Abstractions.Repositories
{
    public interface IProductReadRepository
    {
        Task<Response<ProductReadModel>> GetByIdAsync(string id);

        Task<Response<List<ProductReadModel>>> GetByNameAsync(string brand);

        Task<Response<List<ProductReadModel>>> GetAllAsync();
    }
}
