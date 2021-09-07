using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Vue.Splash_API.Services.Storage
{
    public interface IStorageService
    {
        Task<string> Save(IFormFile file);
        Task<Stream> GetStream(string path);
        Task Delete(string path);
    }
}