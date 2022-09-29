namespace BlazorMovies.Server.Helpers.Interfaces
{
    public interface IFileService
    {
        Task<string> SaveFile(byte[] content, string extension, string containerName);
        Task DeleteFile(string fileRoute, string containerName);
        Task<string> EditFile(byte[] content, string extension, string containerName, string fileRoute);
    }
}
