using System.Threading.Tasks;
using Vue.Splash_API.Dtos;

namespace Vue.Splash_API.Services.UserPhotos;

public interface IUserPhotosService
{
    Task<PhotoReadDto?> GetUserPhoto(int id, string userId);
    Task<string?> GetUserPhotoPath(int id, string userId);
    Task<string?> GetUserPhotoThumbnail(int photoId, string userId);
    Task<bool> PhotoExistsAndOwnedByUser(int photoId, string userId);
}