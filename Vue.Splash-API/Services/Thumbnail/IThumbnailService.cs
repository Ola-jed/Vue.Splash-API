using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Vue.Splash_API.Services.Thumbnail
{
    public interface IThumbnailService
    {
        Task<IFormFile> ReduceQuality(IFormFile baseFile);
    }
}