using AutoMapper;
using Eciton.Application.ReadModels;
using Eciton.Domain.Entities.Identity;

namespace Eciton.Application.MapperProfiles;

public class RoleMappingProfile : Profile
{
    public RoleMappingProfile()
    {
        CreateMap<AppRole, RoleReadModel>();
    }
}
