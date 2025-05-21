using ShopApp.Data.DTO;
using ShopApp.Data.Entities;

namespace ShopApp.Data.MappingProfiles;

using AutoMapper;

public class EmployeeProfile : Profile
{
    public EmployeeProfile()
    {
        CreateMap<EmployeeDto, Employee>();
        CreateMap<Employee, EmployeeDto>();
        CreateMap<RegisterDto, EmployeeDto>();
    }
}