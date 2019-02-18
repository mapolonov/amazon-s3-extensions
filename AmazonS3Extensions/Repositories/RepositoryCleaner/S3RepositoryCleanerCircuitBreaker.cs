using System;
using System.Threading.Tasks;
using AmazonS3Extensions.Contract;
using AmazonS3Extensions.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AmazonS3Extensions.Repositories.RepositoryCleaner
{
    public class S3RepositoryCleanerCircuitBreaker : S3RepositoryCircuitBreakerBase, IRepositoryCleaner<S3FileKey>
    {
        private readonly IRepositoryCleaner<S3FileKey> _repositoryCleaner;

        public S3RepositoryCleanerCircuitBreaker(
            IRepositoryCleaner<S3FileKey> repositoryCleaner,
            ILogger<S3RepositoryCleanerCircuitBreaker> logger,
            IOptions<CircuitBreakerOptions> circuitBreakerOptionsAccessor)
            : base(logger, circuitBreakerOptionsAccessor)
        {
            _repositoryCleaner = repositoryCleaner ?? throw new ArgumentNullException(nameof(repositoryCleaner));
        }

        public async Task DeleteAsync(S3FileKey key)
        {
            await CircuitBreakerPolicy.ExecuteAsync(() => _repositoryCleaner.DeleteAsync(key));
        }
    }
}
