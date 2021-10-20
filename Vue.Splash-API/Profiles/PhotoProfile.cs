using AutoMapper;
using Vue.Splash_API.Dtos;
using Vue.Splash_API.Models;

namespace Vue.Splash_API.Profiles
{
    public class PhotoProfile : Profile
    {
        public PhotoProfile()
        {
            CreateMap<PhotoCreateDto, Photo>();
            CreateMap<PhotoUpdateDto, Photo>();
            CreateMap<Photo, PhotoReadDto>();
        }
    }
}