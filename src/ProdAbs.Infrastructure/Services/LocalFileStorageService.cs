using ProdAbs.Application.Interfaces;
using ProdAbs.SharedKernel;
using System.IO;
using System.Threading.Tasks;

namespace ProdAbs.Infrastructure.Services
{
    public class LocalFileStorageService : IFileStorageService
    {
        public Task<Result> DeleteAsync(string storageLocation)
        {
            throw new System.NotImplementedException();
        }

        public Task<Result<Stream>> GetAsync(string storageLocation)
        {
            throw new System.NotImplementedException();
        }

        public Task<Result<string>> UploadAsync(Stream fileStream, string fileName, string contentType)
        {
            throw new System.NotImplementedException();
        }
    }
}