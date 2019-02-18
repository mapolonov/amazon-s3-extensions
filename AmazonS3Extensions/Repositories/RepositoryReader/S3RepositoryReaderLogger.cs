using System;
using System.Threading.Tasks;
using AmazonS3Extensions.Contract;
using AmazonS3Extensions.Infrastructure;
using AmazonS3Extensions.Models;
using Microsoft.Extensions.Logging;

namespace AmazonS3Extensions.Repositories.RepositoryReader
{
    public class S3RepositoryReaderLogger<TValue> : S3RepositoryLoggerBase, IRepositoryReader<S3FileKey, TValue>
    {
        private readonly IRepositoryReader<S3FileKey, TValue> _s3ClientReader;

        public S3RepositoryReaderLogger(
            IRepositoryReader<S3FileKey, TValue> s3ClientReader,
            ILogger<S3RepositoryReaderLogger<TValue>> logger,
            IBucketNameConstructor bucketNameConstructor) 
            : base(logger, bucketNameConstructor)
        {
            _s3ClientReader = s3ClientReader ?? throw new ArgumentNullException(nameof(s3ClientReader));
        }

        public async Task<TValue> GetAsync(S3FileKey key, bool includeMetadata)
        {
            TValue response = default(TValue);

            await ExcecuteAsync(async () => response = await _s3ClientReader.GetAsync(key, includeMetadata), key);

            return response;
        }
    }
}
