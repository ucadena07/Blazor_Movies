using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using BlazorMovies.Server.Helpers.Interfaces;

namespace BlazorMovies.Server.Helpers
{
    public class FileAzureService : IFileAzureService
    {
        private string connectionString;
        public FileAzureService(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("AzureConnection");
        }
        public async Task DeleteFile(string fileRoute, string containerName)
        {
            if (string.IsNullOrEmpty(fileRoute))
            {
                return;
            }
            var client = new BlobContainerClient(connectionString, containerName);
            await client.CreateIfNotExistsAsync();
            var fileName = Path.GetFileName(fileRoute);
            var blob = client.GetBlobClient(fileName);
            await blob.DeleteIfExistsAsync();
        }

        public async Task<string> EditFile(byte[] content, string extension, string containerName, string fileRoute)
        {
            await DeleteFile(fileRoute, containerName);
            return await SaveFile(content, extension, containerName);
        }

        public async Task<string> SaveFile(byte[] content, string extension, string containerName)
        {
            var client = new BlobContainerClient(connectionString, containerName);
            await client.CreateIfNotExistsAsync();
            client.SetAccessPolicy(Azure.Storage.Blobs.Models.PublicAccessType.Blob);
            var fileName = $"{Guid.NewGuid()}{extension}";
            var blob = client.GetBlobClient(fileName);
            using (var ms = new MemoryStream(content))
            {
                await blob.UploadAsync(ms);
            }
            return blob.Uri.ToString();
        }
    }
}
