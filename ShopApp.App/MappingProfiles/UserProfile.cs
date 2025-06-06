using ShopApp.Data.DTO;
using ShopApp.Data.Entities;

namespace ShopApp.MappingProfiles;

using AutoMapper;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserDto, User>();
        CreateMap<User, UserDto>();
        CreateMap<RegisterDto, UserDto>();
    }
}