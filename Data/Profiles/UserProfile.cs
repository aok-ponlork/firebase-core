using AutoMapper;
using Firebase_Auth.Data.Entities.Authentication;
using Firebase_Auth.Data.Models.Authentication;


namespace Firebase_Auth.Data.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserModel>().ReverseMap();
    }
}
