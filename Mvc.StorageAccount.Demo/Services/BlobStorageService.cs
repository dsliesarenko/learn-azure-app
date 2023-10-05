using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using Mvc.StorageAccount.Demo.Interfaces;

namespace Mvc.StorageAccount.Demo.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private string containerName = "attendeeimages";

        public BlobStorageService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public async Task<string> UploadBlob(IFormFile file, string imageName, string? originalBlobName = null)
        {
            var blobName = $"{imageName}{Path.GetExtension(file.FileName)}";
            var container = _blobServiceClient.GetBlobContainerClient(containerName);

            if (!string.IsNullOrEmpty(originalBlobName)) 
            {
                await RemoveBlob(originalBlobName);
            }

            using var stream = file.OpenReadStream();
            var blob = container.GetBlobClient(blobName);
            await blob.UploadAsync(stream, true);

            return blobName;
        }

        public string GetBlobUrl(string imageName)
        {
            var container = _blobServiceClient.GetBlobContainerClient(containerName);

            var blob = container.GetBlobClient(imageName);

            BlobSasBuilder blobSasBuilder = new()
            {
                BlobContainerName = blob.BlobContainerName,
                BlobName = blob.Name,
                ExpiresOn = DateTime.UtcNow.AddMinutes(2),
                Protocol = SasProtocol.Https,
                Resource = "b"
            };

            blobSasBuilder.SetPermissions(BlobAccountSasPermissions.Read);

            return blob.GenerateSasUri(blobSasBuilder).ToString();
        }

        public async Task RemoveBlob(string imageName)
        {
            var container = _blobServiceClient.GetBlobContainerClient(containerName);
            var blob = container.GetBlobClient(imageName);

            await blob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);
        }
    }
}
