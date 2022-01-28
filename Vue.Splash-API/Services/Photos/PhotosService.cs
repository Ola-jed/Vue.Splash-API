using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using FluentPaginator.Lib.Extensions;
using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;
using Microsoft.EntityFrameworkCore;
using Vue.Splash_API.Data;
using Vue.Splash_API.Dtos;
using Vue.Splash_API.Extensions;
using Vue.Splash_API.Models;

namespace Vue.Splash_API.Services.Photos;

public class PhotosService : IPhotosService
{
    private readonly SplashContext _context;
    private readonly IMapper _mapper;

    public PhotosService(SplashContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PhotoReadDto> CreatePhoto(Photo photo)
    {
        var photoEntry = _context.Photos.Add(photo);
        await _context.SaveChangesAsync();
        return _mapper.Map<PhotoReadDto>(photoEntry.Entity);
    }

    public async Task<UrlPage<PhotoReadDto>> Find(Expression<Func<Photo, bool>> predicate,
        UrlPaginationParameter urlPaginationParameter)
    {
        return await Task.Run(() => _context.Photos.AsNoTracking()
            .Where(predicate)
            .To<PhotoReadDto>()
            .UrlPaginate(urlPaginationParameter, p => p.Id));
    }

    public async Task UpdatePhoto(int id, PhotoUpdateDto photoUpdateDto)
    {
        var photo = await _context.Photos.FindAsync(id);
        _mapper.Map(photoUpdateDto, photo);
        await _context.SaveChangesAsync();
    }

    public async Task DeletePhoto(int id)
    {
        var photo = await _context.Photos.FindAsync(id);
        if (photo == null)
        {
            return;
        }

        _context.Photos.Remove(photo);
        await _context.SaveChangesAsync();
    }
}