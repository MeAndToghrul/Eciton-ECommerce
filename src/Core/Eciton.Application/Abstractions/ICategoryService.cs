using Eciton.Application.DTOs.Category;
using Eciton.Application.ReadModels;
using Eciton.Application.ResponceObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eciton.Application.Abstractions
{
    public interface ICategoryService
    {
        Task<Response>  CreateAsync(CreateCategoryDTO categoryDTO);
        Task<Response<CategoryReadModel>>  GetByIdAsync(string id);
    }
}
