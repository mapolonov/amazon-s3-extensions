using AmazonS3Extensions.Models;

namespace AmazonS3Extensions.Infrastructure
{
    public interface IBucketNameConstructor
    {
        string GetBucketName(S3BaseKey key);
    }
}
