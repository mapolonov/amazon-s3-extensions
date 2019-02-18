using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AmazonS3Extensions.Contract;
using AmazonS3Extensions.Infrastructure;
using AmazonS3Extensions.Models;
using Microsoft.Extensions.Logging;

namespace AmazonS3Extensions.Repositories.HistoryRepositoryReader
{
    public class S3HistoryRepositoryReaderLogger<TValue> : S3RepositoryLoggerBase,
        IHistoryRepositoryReader<S3FileKey, TValue>
    {
        private readonly IHistoryRepositoryReader<S3FileKey, TValue> _repositoryHistoryReader;

        public S3HistoryRepositoryReaderLogger(
            IHistoryRepositoryReader<S3FileKey, TValue> repositoryHistoryReader,
            ILogger<S3HistoryRepositoryReaderLogger<TValue>> logger,
            IBucketNameConstructor bucketNameConstructor)
            : base(logger, bucketNameConstructor)
        {
            _repositoryHistoryReader = repositoryHistoryReader ??
                                       throw new ArgumentNullException(nameof(repositoryHistoryReader));
        }

        public async Task<IEnumerable<TValue>> GetHistoryAsync(S3FileKey key, bool includeMetadata)
        {
            IEnumerable<TValue> response = Enumerable.Empty<TValue>();

            await ExcecuteAsync(async () => response = await _repositoryHistoryReader.GetHistoryAsync(key, includeMetadata), key);

            return response;
        }

        public async Task<IEnumerable<S3FileKey>> GetS3FileKeyVersionsAsync(S3FileKey key)
        {
            IEnumerable<S3FileKey> response = Enumerable.Empty<S3FileKey>();

            await ExcecuteAsync(async () => response = await _repositoryHistoryReader.GetS3FileKeyVersionsAsync(key), key);

            return response;
        }
    }
}