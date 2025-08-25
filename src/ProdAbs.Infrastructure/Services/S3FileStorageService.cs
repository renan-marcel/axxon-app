
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using ProdAbs.Application.Interfaces;
using ProdAbs.SharedKernel;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace ProdAbs.Infrastructure.Services
{
    public class S3FileStorageService : IFileStorageService
    {
        private readonly AmazonS3Client _s3Client;
        private readonly string _bucketName;

        public S3FileStorageService(IConfiguration configuration, IHostEnvironment env)
        {
            var s3Config = new AmazonS3Config
            {
                ServiceURL = "http://localhost:4566",
                ForcePathStyle = true,
                AuthenticationRegion = "us-east-1"
            };
            _s3Client = new AmazonS3Client("test", "test", s3Config);
            _bucketName = configuration["AWS:S3:BucketName"];
            _bucketName = "test";
        }

        public async Task<Result> DeleteAsync(string storageLocation)
        {
            try
            {
                var deleteObjectRequest = new DeleteObjectRequest
                {
                    BucketName = _bucketName,
                    Key = storageLocation
                };

                await _s3Client.DeleteObjectAsync(deleteObjectRequest);
                return Result.Ok();
            }
            catch (AmazonS3Exception e)
            {
                return Result.Fail($"Error deleting file from S3: {e.Message}");
            }
        }

        public async Task<Result<Stream>> GetAsync(string storageLocation)
        {
            try
            {
                var getObjectRequest = new GetObjectRequest
                {
                    BucketName = _bucketName,
                    Key = storageLocation
                };

                var response = await _s3Client.GetObjectAsync(getObjectRequest);
                return Result.Ok<Stream>(response.ResponseStream);
            }
            catch (AmazonS3Exception e)
            {
                return Result.Fail<Stream>($"Error getting file from S3: {e.Message}");
            }
        }

        public async Task<Result<string>> UploadAsync(Stream fileStream, string fileName, string contentType)
        {
            try
            {
                var putObjectRequest = new PutObjectRequest
                {
                    BucketName = _bucketName,
                    Key = fileName,
                    InputStream = fileStream,
                    ContentType = contentType
                };

                await _s3Client.PutObjectAsync(putObjectRequest);
                return Result.Ok<string>(fileName);
            }
            catch (AmazonS3Exception e)
            {
                return Result.Fail<string>($"Error uploading file to S3: {e.Message}");
            }
        }
    }
}
