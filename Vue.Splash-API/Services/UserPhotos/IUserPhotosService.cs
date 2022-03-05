using System.Threading.Tasks;
using Vue.Splash_API.Dtos;

namespace Vue.Splash_API.Services.UserPhotos;

public interface IUserPhotosService
{
    Task<PhotoReadDto?> GetUserPhoto(int id, int userId);
    Task<string?> GetUserPhotoPath(int id, int userId);
    Task<string?> GetUserPhotoThumbnail(int photoId, int userId);
    Task<bool> PhotoExistsAndOwnedByUser(int photoId, int userId);
}