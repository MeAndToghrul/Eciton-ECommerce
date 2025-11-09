using AutoMapper;
using Eciton.Application.DTOs.Product;
using Eciton.Domain.Entities.Entity;
namespace Eciton.Application.MapperProfiles;
public class ProductMappingProfile : Profile
{
    public ProductMappingProfile()
    {
        CreateMap<CreateProductDTO, Product>()
            .ForMember(dest => dest.ProductImage, opt => opt.Ignore());
    }
}
