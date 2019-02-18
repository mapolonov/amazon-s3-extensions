using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using AmazonS3Extensions.Common;
using AmazonS3Extensions.Contract;
using AmazonS3Extensions.Extensions;
using AmazonS3Extensions.Infrastructure;
using AmazonS3Extensions.Models;

namespace AmazonS3Extensions.Repositories.HistoryRepositoryReader
{
    public class S3HistoryRepositoryReader<TValue> : S3RepositoryBase, IHistoryRepositoryReader<S3FileKey, TValue>
    {
        private readonly IRepositoryReader<S3FileKey, TValue> _repositoryReader;
        private readonly int _maxConcurrentTasks;

        public S3HistoryRepositoryReader(
            IRepositoryReader<S3FileKey, TValue> repositoryReader,
            int maxConcurrentTasks,
            IBucketNameConstructor bucketNameConstructor,
            IAmazonS3 amazonS3) : base(bucketNameConstructor, amazonS3)
        {
            _repositoryReader = repositoryReader ?? throw new ArgumentNullException(nameof(repositoryReader));
            _maxConcurrentTasks = maxConcurrentTasks;
        }

        public async Task<IEnumerable<TValue>> GetHistoryAsync(S3FileKey key, bool includeMetadata)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            var fileVersions = await GetS3ObjectVersions(key);
            var recentVersions = fileVersions.TakeWhile(item => !item.IsDeleteMarker).ToList();
            var recentFileKeys = recentVersions.Select(x => x.ToVersionedS3FileKey(key)).ToList();
            var recentFiles = await GetOrderedVersionsAsync(recentFileKeys, includeMetadata);

            return recentFiles;
        }

        public async Task<IEnumerable<S3FileKey>> GetS3FileKeyVersionsAsync(S3FileKey key)
        {
            if(key == null) throw new ArgumentNullException(nameof(key));

            var objectVersions = await GetS3ObjectVersions(key);
            return objectVersions.Select(x => x.ToVersionedS3FileKey(key));
        }

        private async Task<IEnumerable<S3ObjectVersion>> GetS3ObjectVersions(S3FileKey key)
        {
            var request = new ListVersionsRequest
            {
                BucketName = BucketNameConstructor.GetBucketName(key),
                Prefix = key.Key
            };

            var result = new List<S3ObjectVersion>();
            ListVersionsResponse response;
            do
            {
                response = await AmazonS3.ListVersionsAsync(request);
                result.AddRange(response.Versions);

                request.VersionIdMarker = response.NextVersionIdMarker;
                request.KeyMarker = response.NextKeyMarker;
            } while (response.IsTruncated);

            return result;
        }
        
        private async Task<IEnumerable<TValue>> GetOrderedVersionsAsync(IList<S3FileKey> fileKeys, bool includeMetadata)
        {
            var result = Enumerable.Repeat(default(TValue), fileKeys.Count).ToList();
            var tasks = fileKeys.Select(async (fileKey, index) =>
            {
                 var value = await _repositoryReader.GetAsync(fileKey, includeMetadata);
                 result[index] = value;
            });

            await tasks.ThrottleAsync(_maxConcurrentTasks);

            return result;
        }
    }
}