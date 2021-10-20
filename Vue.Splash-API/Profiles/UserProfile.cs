using AutoMapper;
using Vue.Splash_API.Dtos;
using Vue.Splash_API.Models;

namespace Vue.Splash_API.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<ApplicationUser, UserReadDto>();
        }
    }
}