using System;
using System.Text;
using System.Threading.Tasks;
using Amazon.S3;
using AmazonS3Extensions.Infrastructure;
using AmazonS3Extensions.Models;
using Microsoft.Extensions.Logging;

namespace AmazonS3Extensions.Repositories
{
    public abstract class S3RepositoryLoggerBase
    {
        private readonly ILogger<S3RepositoryLoggerBase> _logger;
        private readonly IBucketNameConstructor _bucketNameConstructor;

        protected S3RepositoryLoggerBase(
            ILogger<S3RepositoryLoggerBase> logger,
            IBucketNameConstructor bucketNameConstructor)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _bucketNameConstructor = bucketNameConstructor ?? throw new ArgumentNullException(nameof(bucketNameConstructor));
        }

        protected async Task ExcecuteAsync(Func<Task> action, S3BaseKey key)
        {
            _logger.LogTrace($"ExcecuteAsync for {key}");

            try
            {
                await action();
            }
            catch (AmazonS3Exception ex)
            {
                var message = new StringBuilder(ex.Message)
                    .AppendLine($"StatusCode: {ex.StatusCode}")
                    .AppendLine($"S3FileKey: {key}");

                ex.Data.Add("S3FileKey", key);

                if (ex.ErrorCode != null &&
                    (ex.ErrorCode.Equals("InvalidAccessKeyId") || ex.ErrorCode.Equals("InvalidSecurity")))
                    _logger.LogError(
                        $"Check the provided AWS credentials for bucket '{_bucketNameConstructor.GetBucketName(key)}' and key '{key.Key}'", ex);
                else
                    _logger.LogError(ex, message.ToString());
                
                throw;
            }
            catch (Exception ex)
            {
                var message = new StringBuilder(ex.Message)
                    .AppendLine($"S3FileKey: {key}");

                _logger.LogError(ex, message.ToString());

                ex.Data.Add("S3FileKey", key);
                throw;
            }
        }
    }
}
