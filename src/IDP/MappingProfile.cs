using AutoMapper;
using IDP.Infrastructure.Entities;
using IDP.Infrastructure.ViewModels;

namespace IDP;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Permission, PermissionViewModel>();
        CreateMap<Permission, PermissionUserViewModel>();
    }
}
