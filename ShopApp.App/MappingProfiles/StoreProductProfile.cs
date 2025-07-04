using AutoMapper;
using ShopApp.Data.DTO;
using ShopApp.Data.Entities;

namespace ShopApp.MappingProfiles;

public class StoreProductProfile : Profile
{
    public StoreProductProfile()
    {
        CreateMap<StoreProduct, StoreProductPriceNumberDto>();
        CreateMap<StoreProduct, StoreProductInfoDto>();
        CreateMap<StoreProductInfoDto, StoreProduct>();
        CreateMap<SaveStoreProductDto, StoreProduct>();
        CreateMap<StoreProductInfoDto, SaveStoreProductDto>();
        
    }
}