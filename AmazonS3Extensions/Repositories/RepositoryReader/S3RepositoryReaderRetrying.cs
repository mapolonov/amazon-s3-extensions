using System;
using System.Threading.Tasks;
using AmazonS3Extensions.Contract;
using AmazonS3Extensions.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AmazonS3Extensions.Repositories.RepositoryReader
{
    public class S3RepositoryReaderRetrying<TValue> : S3RepositoryRetryingBase, IRepositoryReader<S3FileKey, TValue>
    {
        private readonly IRepositoryReader<S3FileKey, TValue> _repositoryReader;

        public S3RepositoryReaderRetrying(
            IRepositoryReader<S3FileKey, TValue> repositoryReader,
            ILogger<S3RepositoryReaderRetrying<TValue>> logger,
            IOptions<RetryOptions> retryOptionsAccessor)
            : base(logger, retryOptionsAccessor)
        {
            _repositoryReader = repositoryReader ?? throw new ArgumentNullException(nameof(repositoryReader));
        }

        public async Task<TValue> GetAsync(S3FileKey key, bool includeMetadata)
        {
            return await RetryPolicy.ExecuteAsync(() => _repositoryReader.GetAsync(key, includeMetadata));
        }
    }
}
