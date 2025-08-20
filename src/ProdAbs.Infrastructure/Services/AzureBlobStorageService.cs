using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using ProdAbs.Application.Interfaces;
using ProdAbs.SharedKernel;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ProdAbs.Infrastructure.Services
{
    public class AzureBlobStorageService : IFileStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName;

        public AzureBlobStorageService(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("blobs");
            _containerName = "teste";//configuration.get("StorageSettings:Azure:ContainerName");
            _blobServiceClient = new BlobServiceClient(connectionString);
        }

        public async Task<Result<string>> UploadAsync(Stream fileStream, string fileName, string contentType)
        {
            try
            {
                var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
                await containerClient.CreateIfNotExistsAsync();

                var blobClient = containerClient.GetBlobClient(fileName);
                await blobClient.UploadAsync(fileStream, true);

                return Result.Ok(fileName);
            }
            catch (Exception ex)
            {
                return Result.Fail<string>(ex.Message);
            }
        }

        public async Task<Result<Stream>> GetAsync(string storageLocation)
        {
            try
            {
                var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
                var blobClient = containerClient.GetBlobClient(storageLocation);

                if (!await blobClient.ExistsAsync())
                {
                    return Result.Fail<Stream>("File not found.");
                }

                var downloadInfo = await blobClient.DownloadAsync();
                return Result.Ok(downloadInfo.Value.Content);
            }
            catch (Exception ex)
            {
                return Result.Fail<Stream>(ex.Message);
            }
        }

        public async Task<Result> DeleteAsync(string storageLocation)
        {
            try
            {
                var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
                var blobClient = containerClient.GetBlobClient(storageLocation);
                await blobClient.DeleteIfExistsAsync();

                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
        }
    }
}
