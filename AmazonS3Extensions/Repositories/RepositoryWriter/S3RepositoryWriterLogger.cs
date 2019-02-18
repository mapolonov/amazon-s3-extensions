using System;
using System.Threading.Tasks;
using AmazonS3Extensions.Contract;
using AmazonS3Extensions.Infrastructure;
using AmazonS3Extensions.Models;
using Microsoft.Extensions.Logging;

namespace AmazonS3Extensions.Repositories.RepositoryWriter
{
    public class S3RepositoryWriterLogger<TValue> : S3RepositoryLoggerBase, IRepositoryWriter<S3FileKey, TValue>
    {
        private readonly IRepositoryWriter<S3FileKey, TValue> _s3ClientWriter;

        public S3RepositoryWriterLogger(
            IRepositoryWriter<S3FileKey, TValue> s3ClientWriter,
            ILogger<S3RepositoryWriterLogger<TValue>> logger,
            IBucketNameConstructor bucketNameConstructor)
            : base(logger, bucketNameConstructor)
        {
            _s3ClientWriter = s3ClientWriter ?? throw new ArgumentNullException(nameof(s3ClientWriter));
        }

        public async Task PostAsync(S3FileKey key, TValue fileContainer)
        {
            await ExcecuteAsync(async () => await _s3ClientWriter.PostAsync(key, fileContainer), key);
        }
    }
}