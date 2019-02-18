using System;
using System.Threading.Tasks;
using AmazonS3Extensions.Contract;
using AmazonS3Extensions.Infrastructure;
using AmazonS3Extensions.Models;
using Microsoft.Extensions.Logging;

namespace AmazonS3Extensions.Repositories.BatchRepositoryCleaner
{
    public class S3BatchRepositoryCleanerLogger : S3RepositoryLoggerBase, IBatchRepositoryCleaner<S3FolderKey>
    {
        private readonly IBatchRepositoryCleaner<S3FolderKey> _batchRepositoryCleaner;

        public S3BatchRepositoryCleanerLogger(
            IBatchRepositoryCleaner<S3FolderKey> batchRepositoryCleaner,
            ILogger<S3BatchRepositoryCleanerLogger> logger,
            IBucketNameConstructor bucketNameConstructor)
            : base(logger, bucketNameConstructor)
        {
            _batchRepositoryCleaner = batchRepositoryCleaner ?? throw new ArgumentNullException(nameof(batchRepositoryCleaner));
        }

        public async Task DeleteAsync(S3FolderKey folderKey)
        {
            await ExcecuteAsync(async () => await _batchRepositoryCleaner.DeleteAsync(folderKey), folderKey);
        }
    }
}
