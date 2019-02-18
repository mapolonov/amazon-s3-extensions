using System;
using System.Collections.Generic;
using System.Net;
using Amazon.S3;
using AmazonS3Extensions.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.CircuitBreaker;

namespace AmazonS3Extensions.Repositories
{
    public abstract class S3RepositoryCircuitBreakerBase
    {
        private readonly ILogger<S3RepositoryCircuitBreakerBase> _logger;
        protected readonly AsyncCircuitBreakerPolicy CircuitBreakerPolicy;

        protected S3RepositoryCircuitBreakerBase(
            ILogger<S3RepositoryCircuitBreakerBase> logger,
            IOptions<CircuitBreakerOptions> circuitBreakerOptionsAccessor)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            if (circuitBreakerOptionsAccessor == null)
                throw new ArgumentNullException(nameof(circuitBreakerOptionsAccessor));
            var circuitBreakerOptions = circuitBreakerOptionsAccessor.Value;

            var policyBuilder = Policy
                .Handle<AmazonS3Exception>()
                .Or<WebException>();

            CircuitBreakerPolicy = policyBuilder.CircuitBreakerAsync(
                circuitBreakerOptions.ExceptionsAllowedBeforeBreaking,
                TimeSpan.FromMilliseconds(circuitBreakerOptions.MillisecondsOfBreak),
                OnBreak,
                OnReset);
        }

        private void OnBreak(Exception exception, TimeSpan timespan, IDictionary<string, object> context)
        {
            LogError("Circuit opened for " + timespan, exception, context);
        }

        private void OnReset(IDictionary<string, object> context)
        {
            LogInfo("Circuit closed", context);
        }

        private void LogInfo(string message, IDictionary<string, object> context)
        {
            _logger.LogInformation(message, context);
        }

        private void LogError(string message, Exception ex, IDictionary<string, object> context)
        {
            _logger.LogError(ex, message, context);
        }
    }
}