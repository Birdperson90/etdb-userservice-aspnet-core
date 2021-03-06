using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Etdb.UserService.Domain.ValueObjects;
using Etdb.UserService.Services.Abstractions;
using Etdb.UserService.Services.Abstractions.Models;

namespace Etdb.UserService.Services
{
    public class AzureProfileImageStorageService : IProfileImageStorageService
    {
        private readonly BlobServiceClient blobServiceClient;
        private const string ContainerName = "profileimages";

        public AzureProfileImageStorageService(BlobServiceClient blobServiceClient)
        {
            this.blobServiceClient = blobServiceClient;
        }

        public async Task StoreAsync(StorableImage storableImage)
        {
            var containerClient = this.blobServiceClient.GetBlobContainerClient(ContainerName);

            await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

            var blobClient =
                containerClient.GetBlobClient($"{storableImage.UserId}/{storableImage.ProfileImage.Name}");

            await using var memoryStream = new MemoryStream(storableImage.Image.ToArray());

            await blobClient.UploadAsync(memoryStream, new BlobHttpHeaders
            {
                ContentType = storableImage.ProfileImage.MediaType
            });
        }

        public async Task RemoveAsync(ProfileImage profileImage, Guid userId)
        {
            var containerClient = this.blobServiceClient.GetBlobContainerClient(ContainerName);

            var blobClient =
                containerClient.GetBlobClient($"{userId}/{profileImage.Name}");

            await blobClient.DeleteAsync();
        }
    }
}