using System;
using System.Threading.Tasks;
using AmazonS3Extensions.Contract;
using AmazonS3Extensions.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AmazonS3Extensions.Repositories.RepositoryReader
{
    public class S3RepositoryReaderCircuitBreaker<TValue> : S3RepositoryCircuitBreakerBase, IRepositoryReader<S3FileKey, TValue>
    {
        private readonly IRepositoryReader<S3FileKey, TValue> _repositoryReader;

        public S3RepositoryReaderCircuitBreaker(
            IRepositoryReader<S3FileKey, TValue> repositoryReader,
            ILogger<S3RepositoryReaderCircuitBreaker<TValue>> logger,
            IOptions<CircuitBreakerOptions> circuitBreakerOptionsAccessor)
            : base(logger, circuitBreakerOptionsAccessor)
        {
            _repositoryReader = repositoryReader ?? throw new ArgumentNullException(nameof(repositoryReader));
        }

        public async Task<TValue> GetAsync(S3FileKey key, bool includeMetadata)
        {
            return await CircuitBreakerPolicy.ExecuteAsync(() =>
                _repositoryReader.GetAsync(key, includeMetadata));
        }
    }
}
