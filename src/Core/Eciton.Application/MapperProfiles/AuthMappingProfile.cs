using AutoMapper;
using Eciton.Application.DTOs.Auth;
using Eciton.Domain.Entities.Identity;

namespace Eciton.Application.MapperProfiles;
public class AuthMappingProfile : Profile
{
    public AuthMappingProfile()
    {
        CreateMap<RegisterDTO, AppUser>()
                .ForMember(dest => dest.FullName, opt =>
                    opt.MapFrom(src => $"{src.Name.Trim()} {src.Surname.Trim()}"))

                .ForMember(dest => dest.Email, opt =>
                    opt.MapFrom(src => src.Email.Trim()))

                .ForMember(dest => dest.NormalizedEmail, opt =>
                    opt.MapFrom(src => src.Email.ToUpper().Trim()))

                .ForMember(dest => dest.PasswordHash, opt =>
                    opt.Ignore())

                .ForMember(dest => dest.RoleId, opt =>
                    opt.Ignore())

                .ForMember(dest => dest.Role, opt =>
                    opt.Ignore())

                .ForMember(dest => dest.IsEmailConfirmed, opt =>
                    opt.MapFrom(src => false))

                .ForMember(dest => dest.LockoutEnabled, opt =>
                    opt.MapFrom(src => true))

                .ForMember(dest => dest.LockoutEnd, opt =>
                    opt.Ignore())

                .ForMember(dest => dest.AccessFailedCount, opt =>
                    opt.MapFrom(src => 0));
    }
}
