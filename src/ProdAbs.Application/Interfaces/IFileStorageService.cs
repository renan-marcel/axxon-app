
using ProdAbs.SharedKernel;
using System.IO;
using System.Threading.Tasks;

namespace ProdAbs.Application.Interfaces
{
    public interface IFileStorageService
    {
        Task<Result<string>> UploadAsync(Stream fileStream, string fileName, string contentType);
        Task<Result<Stream>> GetAsync(string storageLocation);
        Task<Result> DeleteAsync(string storageLocation);
    }
}
