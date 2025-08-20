using Microsoft.Extensions.Configuration;
using ProdAbs.Application.Interfaces;
using ProdAbs.SharedKernel;

namespace ProdAbs.Infrastructure.Services
{
    public class LocalFileStorageService : IFileStorageService
    {
        private readonly string _storagePath;

        public LocalFileStorageService(IConfiguration configuration)
        {
            _storagePath = configuration.GetConnectionString("StorageSettings:Local:BasePath") ?? "uploads";
            if (!Directory.Exists(_storagePath))
            {
                Directory.CreateDirectory(_storagePath);
            }
        }

        public async Task<Result<string>> UploadAsync(Stream fileStream, string fileName, string contentType)
        {
            try
            {
                var storageFileName = $"{Guid.NewGuid()}{Path.GetExtension(fileName)}";
                var filePath = Path.Combine(_storagePath, storageFileName);

                await using var stream = new FileStream(filePath, FileMode.Create);
                await fileStream.CopyToAsync(stream);

                return Result.Ok(storageFileName);
            }
            catch (Exception ex)
            {
                return Result.Fail<string>(ex.Message);
            }
        }

        public Task<Result<Stream>> GetAsync(string storageLocation)
        {
            try
            {
                var filePath = Path.Combine(_storagePath, storageLocation);
                if (!File.Exists(filePath))
                {
                    return Task.FromResult(Result.Fail<Stream>("File not found."));
                }

                var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                return Task.FromResult(Result.Ok<Stream>(stream));
            }
            catch (Exception ex)
            {
                return Task.FromResult(Result.Fail<Stream>(ex.Message));
            }
        }

        public Task<Result> DeleteAsync(string storageLocation)
        {
            try
            {
                var filePath = Path.Combine(_storagePath, storageLocation);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                return Task.FromResult(Result.Ok());
            }
            catch (Exception ex)
            {
                return Task.FromResult(Result.Fail(ex.Message));
            }
        }
    }
}
