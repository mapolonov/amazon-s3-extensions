using System;
using System.Threading.Tasks;
using AmazonS3Extensions.Contract;
using AmazonS3Extensions.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AmazonS3Extensions.Repositories.RepositoryCleaner
{
    public class S3RepositoryCleanerRetrying : S3RepositoryRetryingBase, IRepositoryCleaner<S3FileKey>
    {
        private readonly IRepositoryCleaner<S3FileKey> _repositoryCleaner;

        public S3RepositoryCleanerRetrying(
            IRepositoryCleaner<S3FileKey> repositoryCleaner,
            ILogger<S3RepositoryCleanerRetrying> logger,
            IOptions<RetryOptions> retryOptionsAccessor)
            : base(logger, retryOptionsAccessor)
        {
            _repositoryCleaner = repositoryCleaner ?? throw new ArgumentNullException(nameof(repositoryCleaner));
        }

        public async Task DeleteAsync(S3FileKey key)
        {
            await RetryPolicy.ExecuteAsync(() => _repositoryCleaner.DeleteAsync(key));
        }
    }
}
