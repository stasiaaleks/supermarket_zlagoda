using ShopApp.Data.DTO;
using ShopApp.Data.Entities;

namespace ShopApp.MappingProfiles;

using AutoMapper;

public class EmployeeProfile : Profile
{
    public EmployeeProfile()
    {
        CreateMap<EmployeeDto, Employee>();
        CreateMap<CreateEmployeeDto, Employee>();
        CreateMap<Employee, EmployeeDto>();
        CreateMap<RegisterDto, EmployeeDto>();
        CreateMap<Employee, EmployeeContactsDto>();
    }
}