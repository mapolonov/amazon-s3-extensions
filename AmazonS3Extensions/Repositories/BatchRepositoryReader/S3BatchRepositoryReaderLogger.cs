using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AmazonS3Extensions.Contract;
using AmazonS3Extensions.Infrastructure;
using AmazonS3Extensions.Models;
using Microsoft.Extensions.Logging;

namespace AmazonS3Extensions.Repositories.BatchRepositoryReader
{
    public class S3BatchRepositoryReaderLogger : S3RepositoryLoggerBase, IBatchRepositoryReader<S3FileKey, S3FolderKey>
    {
        private readonly IBatchRepositoryReader<S3FileKey, S3FolderKey> _batchRepositoryReader;

        public S3BatchRepositoryReaderLogger(
            IBatchRepositoryReader<S3FileKey, S3FolderKey> batchRepositoryReader,
            ILogger<S3BatchRepositoryReaderLogger> logger,
            IBucketNameConstructor bucketNameConstructor)
            : base(logger, bucketNameConstructor)
        {
            _batchRepositoryReader = batchRepositoryReader 
                ?? throw new ArgumentNullException(nameof(batchRepositoryReader));
        }

        public async Task<IEnumerable<S3FileKey>> GetAllKeysAsync(S3FolderKey filter)
        {
            IEnumerable<S3FileKey> response = Enumerable.Empty<S3FileKey>();

            await ExcecuteAsync(async () => response = await _batchRepositoryReader.GetAllKeysAsync(filter), filter);

            return response;
        }
    }
}
