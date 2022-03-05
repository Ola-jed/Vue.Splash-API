using AutoMapper;
using Vue.Splash_API.Dtos;
using Vue.Splash_API.Models;

namespace Vue.Splash_API.Profiles;

public class ApplicationUserProfile : Profile
{
    public ApplicationUserProfile()
    {
        CreateMap<ApplicationUser, UserReadDto>();
    }
}