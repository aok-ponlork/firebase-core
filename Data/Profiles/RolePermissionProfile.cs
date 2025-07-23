using AutoMapper;
using Firebase_Auth.Data.Entities.Authentication;
using Firebase_Auth.Data.Models.Authorization;

namespace Firebase_Auth.Data.Profiles;

public class RolePermissionProfile : Profile
{
    public RolePermissionProfile()
    {
        CreateMap<Role, RoleDto>().ReverseMap();
        CreateMap<Role, RoleCreateDto>().ReverseMap();
    }
}