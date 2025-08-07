using AutoMapper;
using Eciton.Application.DTOs.CategoryField;
using Eciton.Domain.Entities.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eciton.Application.MapperProfiles
{
    public class CategoryFieldMappingProfile : Profile
    {
        public CategoryFieldMappingProfile()
        {
            CreateMap<CreateCategoryFieldDTO,CategoryField>();            
        }    
    }
}
