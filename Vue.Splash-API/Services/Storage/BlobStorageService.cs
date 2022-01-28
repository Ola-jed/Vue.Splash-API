using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Vue.Splash_API.Services.Storage;

public class BlobStorageService : IStorageService
{
    private readonly BlobContainerClient _blobContainerClient;

    public BlobStorageService(IOptions<BlobStorageSettings> options)
    {
        var azureBlobKey = options.Value.AzureBlobKey;
        var containerName = options.Value.ContainerName;
        var blobServiceClient = new BlobServiceClient(azureBlobKey);
        _blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
    }

    public async Task<string> Save(IFormFile file)
    {
        var fileName = DateTime.Now.Ticks.ToString();
        var blobClient = _blobContainerClient.GetBlobClient(fileName);
        await using var fileStream = file.OpenReadStream();
        await blobClient.UploadAsync(fileStream, true);
        fileStream.Close();
        return fileName;
    }

    public async Task<Stream> GetStream(string path)
    {
        var blob = _blobContainerClient.GetBlobClient(path);
        BlobDownloadInfo blobData = await blob.DownloadAsync();
        return blobData.Content;
    }

    public async Task Delete(string path)
    {
        var blob = _blobContainerClient.GetBlobClient(path);
        await blob.DeleteIfExistsAsync();
    }
}