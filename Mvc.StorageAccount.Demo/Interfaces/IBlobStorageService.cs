namespace Mvc.StorageAccount.Demo.Interfaces
{
    public interface IBlobStorageService
    {
        string GetBlobUrl(string imageName);
        Task RemoveBlob(string imageName);
        Task<string> UploadBlob(IFormFile file, string imageName, string? originalBlobName = null);
    }
}