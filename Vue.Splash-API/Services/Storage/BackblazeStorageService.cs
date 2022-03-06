using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System;
using Backblaze_Client;
using Microsoft.Extensions.Options;
using Serilog;

namespace Vue.Splash_API.Services.Storage;

public class BackblazeStorageService : IStorageService
{
    private readonly BackblazeClient _client;
    private bool _accountAuthorized;

    public BackblazeStorageService(IOptions<BackblazeConfig> config)
    {
        var logger = Log.ForContext(GetType());
        _client = new BackblazeClient(config.Value,logger);
    }

    public async Task<string> Save(IFormFile file)
    {
        await CheckAuthorization();
        await using var fileStream = file.OpenReadStream();
        var bytes = new byte[file.Length];
        await fileStream.ReadAsync(bytes.AsMemory(0, (int)file.Length));
        var fileName = DateTime.Now.Ticks + Path.GetExtension(file.FileName);
        return await _client.Upload(fileName, bytes);
    }

    public async Task<Stream> GetStream(string path)
    {
        await CheckAuthorization();
        return await _client.Download(path);
    }

    public async Task Delete(string path)
    {
        await CheckAuthorization();
        await _client.Delete(path, await _client.GetFileName(path));
    }

    private async Task CheckAuthorization()
    {
        if (!_accountAuthorized)
        {
            await _client.AuthorizeAccount();
            _accountAuthorized = true;
        }
    }
}