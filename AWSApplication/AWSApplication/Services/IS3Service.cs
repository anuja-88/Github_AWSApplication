using AWSApplication.Data;
using System.Threading.Tasks;

namespace AWSApplication.Services
{
    public interface IS3Service
    {

        Task<S3Response> CreateBucketAsync(string name);

        Task UploadFileAsync(string bucketName);

        Task DeleteObjectNonVersionedBucketAsync(string bucketName);
    }
}