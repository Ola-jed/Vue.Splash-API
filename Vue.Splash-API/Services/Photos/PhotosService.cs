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
        var urlPage = await _context.Photos
            .AsNoTracking()
            .Where(predicate)
            .AsyncUrlPaginate(urlPaginationParameter, p => p.Id);
    
        return urlPage.Map(x => _mapper.Map<PhotoReadDto>(x));
    }

    public async Task UpdatePhoto(int id, PhotoUpdateDto photoUpdateDto)
    {
        await _context.Photos
            .Where(x => x.Id == id)
            .ExecuteUpdateAsync(x => x.SetProperty(p => p.Label, photoUpdateDto.Label)
                .SetProperty(p => p.Description, photoUpdateDto.Description)
            );
    }

    public async Task DeletePhoto(int id)
    {
        await _context.Photos
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync();
    }
}