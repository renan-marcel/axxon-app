using ProdAbs.Application.Interfaces;
using ProdAbs.SharedKernel;

namespace ProdAbs.Infrastructure.Services
{
    public class LocalFileStorageService : IFileStorageService
    {
        private readonly string _storagePath;

        public LocalFileStorageService()
        {
            _storagePath = Path.Combine(Path.GetTempPath(), "ProdAbsStorage");
            if (!Directory.Exists(_storagePath))
            {
                Directory.CreateDirectory(_storagePath);
            }
        }

        public async Task<Result<string>> UploadAsync(Stream fileStream, string fileName, string contentType)
        {
            try
            {
                var storageLocation = Guid.NewGuid().ToString() + Path.GetExtension(fileName);
                var filePath = Path.Combine(_storagePath, storageLocation);

                using (var newFileStream = new FileStream(filePath, FileMode.Create))
                {
                    await fileStream.CopyToAsync(newFileStream);
                }

                return Result.Ok(storageLocation);
            }
            catch (Exception ex)
            {
                return Result.Fail<string>("Failed to upload file: " + ex.Message);
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
                return Task.FromResult(Result.Fail<Stream>("Failed to retrieve file: " + ex.Message));
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
                return Task.FromResult(Result.Fail("Failed to delete file: " + ex.Message));
            }
        }
    }
}
