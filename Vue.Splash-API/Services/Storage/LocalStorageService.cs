using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Vue.Splash_API.Services.Storage
{
    public class LocalStorageService: IStorageService
    {
        private readonly IWebHostEnvironment _env;

        public LocalStorageService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string> Save(IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName);
            var fileName = DateTime.Now.Ticks + extension;
            var pathBuilt = Path.Combine(_env.ContentRootPath, "Images");
            if (!Directory.Exists(pathBuilt))
            {
                Directory.CreateDirectory(pathBuilt);
            }
            var path = Path.Combine(
                Directory.GetCurrentDirectory(),
                "Images",
                fileName);
            await using var stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream);
            return path;
        }

        public Task<Stream> GetStream(string path)
        {
            return Task.FromResult<Stream>(File.OpenRead(path));
        }

        public Task Delete(string path)
        {
            File.Delete(path);
            return Task.CompletedTask;
        }
    }
}