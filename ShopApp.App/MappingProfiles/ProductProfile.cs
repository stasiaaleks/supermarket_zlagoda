using AutoMapper;
using ShopApp.Data.DTO;

namespace ShopApp.MappingProfiles;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<ProductDto, Product>();
        CreateMap<Product, ProductDto>();
        CreateMap<ProductDto, UpdateProductDto>();
        CreateMap<UpdateProductDto, ProductDto>();
        CreateMap<ProductDto, CreateProductDto>();
        CreateMap<CreateProductDto, ProductDto>();
    }
}