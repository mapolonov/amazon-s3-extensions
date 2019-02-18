using System;
using System.Threading.Tasks;
using AmazonS3Extensions.Contract;
using AmazonS3Extensions.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AmazonS3Extensions.Repositories.RepositoryWriter
{
    public class S3RepositoryWriterRetrying<TValue> : S3RepositoryRetryingBase, IRepositoryWriter<S3FileKey, TValue>
    {
        private readonly IRepositoryWriter<S3FileKey, TValue> _repositoryWriter;

        public S3RepositoryWriterRetrying(
            IRepositoryWriter<S3FileKey, TValue> repositoryWriter,
            ILogger<S3RepositoryWriterRetrying<TValue>> logger,
            IOptions<RetryOptions> retryOptionsAccessor) 
            : base(logger, retryOptionsAccessor)
        {
            _repositoryWriter = repositoryWriter ?? throw new ArgumentNullException(nameof(repositoryWriter));
        }

        public async Task PostAsync(S3FileKey key, TValue value)
        {
            await RetryPolicy.ExecuteAsync(() => _repositoryWriter.PostAsync(key, value));
        }
    }
}
