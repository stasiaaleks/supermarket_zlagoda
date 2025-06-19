using AutoMapper;
using ShopApp.Data.DTO;
using ShopApp.Data.Entities;

namespace ShopApp.MappingProfiles;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<CreateCategoryDto, CategoryDto>();
        CreateMap<CategoryDto, CreateCategoryDto>();
        CreateMap<CreateCategoryDto, Category>();
        CreateMap<Category, CreateCategoryDto>();
        CreateMap<CategoryDto, Category>();
        CreateMap<Category, CategoryDto>();
        
    }
}

