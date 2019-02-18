using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Amazon.S3;
using AmazonS3Extensions.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;


namespace AmazonS3Extensions.Repositories
{
    public abstract class S3RepositoryRetryingBase
    {
        private static readonly HttpStatusCode[] HandleHttpStatusCodes =
        {
            HttpStatusCode.ServiceUnavailable,
            HttpStatusCode.BadGateway,
            HttpStatusCode.GatewayTimeout,
            HttpStatusCode.RequestTimeout,
            HttpStatusCode.Conflict,
            HttpStatusCode.PreconditionFailed
        };

        private readonly ILogger<S3RepositoryRetryingBase> _logger;
        protected readonly AsyncRetryPolicy RetryPolicy;

        protected S3RepositoryRetryingBase(
            ILogger<S3RepositoryRetryingBase> logger,
            IOptions<RetryOptions> retryOptionsAccessor)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            var policyBuilder = Policy.Handle<AmazonS3Exception>(ex => HandleHttpStatusCodes.Contains(ex.StatusCode));

            if (retryOptionsAccessor == null)
                throw new ArgumentNullException(nameof(retryOptionsAccessor));
            var retryOptions = retryOptionsAccessor.Value;
            RetryPolicy = policyBuilder.RetryAsync(retryOptions.NumberOfRetries, OnRetryAsync);
        }

        private Task OnRetryAsync(Exception exception, int count, IDictionary<string, object> context)
        {
            LogError($"Retry nr {count} after exception", exception, context);

            return Task.FromResult(0);
        }

        private void LogError(string message, Exception ex, IDictionary<string, object> context) 
            => _logger.LogError(ex, message, context);
    }
}
