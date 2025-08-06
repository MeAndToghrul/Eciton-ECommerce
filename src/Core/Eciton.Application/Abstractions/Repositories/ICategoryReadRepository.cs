using Eciton.Application.ReadModels;
using Eciton.Application.ResponceObject;
namespace Eciton.Application.Abstractions.Repositories;
public interface ICategoryReadRepository
{
    Task<Response<CategoryReadModel>> GetByIdAsync(string id);
}
