using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Vue.Splash_API.Services.Storage
{
    public interface IStorageService
    {
        Task<string> SaveImage(IFormFile file);
        FileStream GetStream(string path);
    }
}