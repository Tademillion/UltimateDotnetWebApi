using AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Company, CompanyDto>()
            .ForMember(c => c.FullAddress,
                opt => opt.MapFrom(x => $"{x.Address} {x.Country}"));
        CreateMap<CompanyForCreationDto, Company>();
        // 
        CreateMap<EmployeeCreationDto, Employee>();
        CreateMap<Employee, EmployeeDto>();
        CreateMap<EmployeeForUpdateDto, Employee>();
        //  reverse mapping for patch
        CreateMap<EmployeeForUpdateDto, Employee>().ReverseMap();
    }
}

