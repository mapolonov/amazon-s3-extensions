using System;
using System.Threading.Tasks;
using AmazonS3Extensions.Contract;
using AmazonS3Extensions.Infrastructure;
using AmazonS3Extensions.Models;
using Microsoft.Extensions.Logging;

namespace AmazonS3Extensions.Repositories.RepositoryCleaner
{
    public class S3RepositoryCleanerLogger : S3RepositoryLoggerBase, IRepositoryCleaner<S3FileKey>
    {
        private readonly IRepositoryCleaner<S3FileKey> _repositoryCleaner;

        public S3RepositoryCleanerLogger(
            IRepositoryCleaner<S3FileKey> repositoryCleaner,
            ILogger<S3RepositoryCleanerLogger> logger,
            IBucketNameConstructor bucketNameConstructor)
            : base(logger, bucketNameConstructor)
        {
            _repositoryCleaner = repositoryCleaner ?? throw new ArgumentNullException(nameof(repositoryCleaner));
        }

        public async Task DeleteAsync(S3FileKey key)
        {
            await ExcecuteAsync(async () => await _repositoryCleaner.DeleteAsync(key), key);
        }
    }
}
