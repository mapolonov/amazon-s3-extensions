using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AmazonS3Extensions.Contract;
using AmazonS3Extensions.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AmazonS3Extensions.Repositories.BatchRepositoryReader
{
    public class S3BatchRepositoryReaderCircuitBreaker : S3RepositoryCircuitBreakerBase,
        IBatchRepositoryReader<S3FileKey, S3FolderKey>
    {
        private readonly IBatchRepositoryReader<S3FileKey, S3FolderKey> _batchRepositoryReader;

        public S3BatchRepositoryReaderCircuitBreaker(
            IBatchRepositoryReader<S3FileKey, S3FolderKey> batchRepositoryReader,
            ILogger<S3BatchRepositoryReaderCircuitBreaker> logger,
            IOptions<CircuitBreakerOptions> circuitBreakerOptionsAccessor)
            : base(logger, circuitBreakerOptionsAccessor)
        {
            _batchRepositoryReader =
                batchRepositoryReader ?? throw new ArgumentNullException(nameof(batchRepositoryReader));
        }

        public async Task<IEnumerable<S3FileKey>> GetAllKeysAsync(S3FolderKey filter)
        {
            return await CircuitBreakerPolicy.ExecuteAsync(() =>
                _batchRepositoryReader.GetAllKeysAsync(filter));
        }
    }
}
