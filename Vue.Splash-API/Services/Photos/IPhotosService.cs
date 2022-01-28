using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;
using Vue.Splash_API.Dtos;
using Vue.Splash_API.Models;
using Vue.Splash_API.Services.UserPhotos;

namespace Vue.Splash_API.Services.Photos;

public interface IPhotosService
{
    Task<PhotoReadDto> CreatePhoto(Photo photo);

    Task<UrlPage<PhotoReadDto>> Find(Expression<Func<Photo, bool>> predicate,
        UrlPaginationParameter urlPaginationParameter);

    Task UpdatePhoto(int id, PhotoUpdateDto photoUpdateDto);
    Task DeletePhoto(int id);
}