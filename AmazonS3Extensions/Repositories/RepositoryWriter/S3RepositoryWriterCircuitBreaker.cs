using System;
using System.Threading.Tasks;
using AmazonS3Extensions.Contract;
using AmazonS3Extensions.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AmazonS3Extensions.Repositories.RepositoryWriter
{
    public class S3RepositoryWriterCircuitBreaker<TValue> : S3RepositoryCircuitBreakerBase, IRepositoryWriter<S3FileKey, TValue>
    {
        private readonly IRepositoryWriter<S3FileKey, TValue> _repositoryWriter;

        public S3RepositoryWriterCircuitBreaker(
            IRepositoryWriter<S3FileKey, TValue> repositoryWriter,
            ILogger<S3RepositoryWriterCircuitBreaker<TValue>> logger,
            IOptions<CircuitBreakerOptions> circuitBreakerOptionsAccessor)
            : base(logger, circuitBreakerOptionsAccessor)
        {
            _repositoryWriter = repositoryWriter ?? throw new ArgumentNullException(nameof(repositoryWriter));
        }

        public async Task PostAsync(S3FileKey key, TValue fileContainer)
        {
            await CircuitBreakerPolicy.ExecuteAsync(() =>
                _repositoryWriter.PostAsync(key, fileContainer));
        }
    }
}
