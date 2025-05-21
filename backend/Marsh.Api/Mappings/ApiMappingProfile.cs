using AutoMapper;
using Marsh.Api.DTOs.Projects;
using Marsh.Api.DTOs.Users;
using Marsh.Api.Models;

namespace Marsh.Api.Mappings;

public class ApiMappingProfile : Profile
{
    public ApiMappingProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<UpdateUserDto, User>()
            .ForAllMembers(opts => opts.Condition((_, _, srcMember) => srcMember != null));
        CreateMap<CreateUserDto, User>()
            .ForMember(dst => dst.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
        
        CreateMap<Project, ProjectDto>();
        CreateMap<UpdateProjectDto, Project>()
            .ForAllMembers ( opts => opts.Condition(( _, _, srcMember ) => srcMember != null ));
        CreateMap<CreateProjectDto, Project>()
            .ForMember(dst => dst.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
    }
}