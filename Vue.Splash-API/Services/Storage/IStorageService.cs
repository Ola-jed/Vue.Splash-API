using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Vue.Splash_API.Services.Storage
{
    public interface IStorageService
    {
        Task<string> Save(IFormFile file);
        Stream GetStream(string path);
        void Delete(string path);
    }
}