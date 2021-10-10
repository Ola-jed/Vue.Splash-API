using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace Vue.Splash_API.Services.Thumbnail
{
    public class ThumbnailService : IThumbnailService
    {
        public async Task<IFormFile> ReduceQuality(IFormFile baseFile)
        {
            var tempImage = await Image.LoadAsync(baseFile.OpenReadStream());
            var encoder = new JpegEncoder
            {
                Quality = 5
            };
            var outputStream = new MemoryStream();
            await tempImage.SaveAsync(outputStream, encoder);
            var ticks = DateTime.Now.Ticks.ToString();
            return new FormFile(outputStream, 0, outputStream.Length,ticks,ticks);
        }
    }
}