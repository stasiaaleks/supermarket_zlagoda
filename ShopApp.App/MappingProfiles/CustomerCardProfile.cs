using AutoMapper;
using ShopApp.Data.DTO;
using ShopApp.Data.Entities;

namespace ShopApp.MappingProfiles;

public class CustomerCardProfile : Profile
{
    public CustomerCardProfile()
    {
        CreateMap<CustomerCard, CreateCustomerCardDto>();
        CreateMap<CreateCustomerCardDto, CustomerCard>();
        CreateMap<CustomerCardDto, CustomerCard>();
        CreateMap<CustomerCard, CustomerCardDto>();
        
    }
}

