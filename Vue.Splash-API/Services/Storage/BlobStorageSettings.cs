namespace Vue.Splash_API.Services.Storage
{
    public record BlobStorageSettings
    {
        public string AzureBlobKey { get; init; }
        public string ContainerName { get; init; }
    }
}