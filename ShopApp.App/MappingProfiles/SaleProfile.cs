using AutoMapper;
using ShopApp.Data.DTO;
using ShopApp.Data.Entities;

namespace ShopApp.MappingProfiles;

public class SaleProfile : Profile
{
    public SaleProfile()
    {
        CreateMap<CreateSaleDto, Sale>();
        CreateMap<CreateSaleDto, SaleDto>();
        CreateMap<SaleDto, Sale>();
        CreateMap<Sale, SaleDto>();
    }
}