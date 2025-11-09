using AutoMapper;
using Eciton.Application.DTOs.Category;
using Eciton.Domain.Entities.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eciton.Application.MapperProfiles
{
    public class CategoryMappingProfile : Profile
    {
        public CategoryMappingProfile()
        {
            CreateMap<CreateCategoryDTO, Category>()
                .ForMember(dest => dest.CategoryImage, opt => opt.Ignore());
        }
    }
}
