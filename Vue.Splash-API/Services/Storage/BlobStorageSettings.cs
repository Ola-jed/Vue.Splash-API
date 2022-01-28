namespace Vue.Splash_API.Services.Storage;

public record BlobStorageSettings
{
    public string AzureBlobKey { get; init; } = string.Empty;
    public string ContainerName { get; init; } = string.Empty;
}