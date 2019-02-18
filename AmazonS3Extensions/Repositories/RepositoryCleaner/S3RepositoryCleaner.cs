using System;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using AmazonS3Extensions.Contract;
using AmazonS3Extensions.Infrastructure;
using AmazonS3Extensions.Models;

namespace AmazonS3Extensions.Repositories.RepositoryCleaner
{
    public class S3RepositoryCleaner : S3RepositoryBase, IRepositoryCleaner<S3FileKey>
    {
        public S3RepositoryCleaner(IAmazonS3 amazonS3,
                               IBucketNameConstructor bucketNameConstructor) : base(bucketNameConstructor, amazonS3)
        {}

        public async Task DeleteAsync(S3FileKey key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            var request = new DeleteObjectRequest
            {
                BucketName = BucketNameConstructor.GetBucketName(key),
                Key = key.Key
            };

            await AmazonS3.DeleteObjectAsync(request);
        }

        public async Task DeleteVersionedAsync(S3FileKey key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            var request = new DeleteObjectRequest
            {
                BucketName = BucketNameConstructor.GetBucketName(key),
                Key = key.Key,
                VersionId = key.VersionId
            };

            await AmazonS3.DeleteObjectAsync(request);
        }
    }
}