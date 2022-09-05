using AutoMapper;
using InCinema.Models.Roles;

namespace InCinema.Profiles;

public class RoleProfile : Profile
{
    public RoleProfile()
    {
        CreateMap<Role, RoleView>();
        CreateMap<RoleCreate, Role>();
        CreateMap<RoleUpdate, Role>();
    }
}