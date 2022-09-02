using AutoMapper;
using InCinema.Models.Users;

namespace InCinema.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserPreview>();
        CreateMap<User, UserView>();
        CreateMap<UserCreate, User>();
        CreateMap<UserUpdate, User>();
    }
}