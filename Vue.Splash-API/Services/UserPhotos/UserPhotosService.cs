using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Vue.Splash_API.Data;
using Vue.Splash_API.Dtos;
using Vue.Splash_API.Extensions;

namespace Vue.Splash_API.Services.UserPhotos;

public class UserPhotosService : IUserPhotosService
{
    private readonly SplashContext _context;

    public UserPhotosService(SplashContext context)
    {
        _context = context;
    }

    public async Task<PhotoReadDto?> GetUserPhoto(int id, int userId)
    {
        return await _context.Photos.AsNoTracking()
            .Where(p => p.ApplicationUserId == userId)
            .To<PhotoReadDto>()
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<string?> GetUserPhotoPath(int id, int userId)
    {
        return await _context.Photos.AsNoTracking()
            .Where(p => p.ApplicationUserId == userId && p.Id == id)
            .Select(p => p.Path)
            .FirstOrDefaultAsync();
    }

    public async Task<string?> GetUserPhotoThumbnail(int photoId, int userId)
    {
        return await _context.Photos.AsNoTracking()
            .Where(p => p.Id == photoId && p.ApplicationUserId == userId)
            .Select(p => p.Thumbnail)
            .FirstOrDefaultAsync();
    }

    public async Task<bool> PhotoExistsAndOwnedByUser(int photoId, int userId)
    {
        return await _context.Photos
            .AnyAsync(p => p.Id == photoId && p.ApplicationUserId == userId);
    }
}