using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AmazonS3Extensions.Contract;
using AmazonS3Extensions.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AmazonS3Extensions.Repositories.BatchRepositoryReader
{
    public class S3BatchRepositoryReaderRetrying : S3RepositoryRetryingBase, IBatchRepositoryReader<S3FileKey, S3FolderKey>
    {
        private readonly IBatchRepositoryReader<S3FileKey, S3FolderKey> _batchRepositoryReader;

        public S3BatchRepositoryReaderRetrying(
            IBatchRepositoryReader<S3FileKey, S3FolderKey> batchRepositoryReader,
            ILogger<S3BatchRepositoryReaderRetrying> logger,
            IOptions<RetryOptions> retryOptionsAccessor)
            : base(logger, retryOptionsAccessor)
        {
            _batchRepositoryReader = batchRepositoryReader ?? throw new ArgumentNullException(nameof(batchRepositoryReader));
        }

        public async Task<IEnumerable<S3FileKey>> GetAllKeysAsync(S3FolderKey filter)
        {
            return await RetryPolicy.ExecuteAsync(() => _batchRepositoryReader.GetAllKeysAsync(filter));
        }
    }
}
