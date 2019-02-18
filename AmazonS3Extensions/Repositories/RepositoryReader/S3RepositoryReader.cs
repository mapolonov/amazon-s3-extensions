using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using AmazonS3Extensions.Contract;
using AmazonS3Extensions.Extensions;
using AmazonS3Extensions.Infrastructure;
using AmazonS3Extensions.Models;

namespace AmazonS3Extensions.Repositories.RepositoryReader
{
    public class S3RepositoryReader<TValue> : S3RepositoryBase, IRepositoryReader<S3FileKey, TValue>
    {
        private readonly IDataContainerConverter<TValue> _containerConverter;

        public S3RepositoryReader(
            IAmazonS3 amazonS3,
            IBucketNameConstructor bucketNameConstructor,
            IDataContainerConverter<TValue> containerConverter) : base(bucketNameConstructor, amazonS3)
        {
            _containerConverter = containerConverter ?? throw new ArgumentNullException(nameof(containerConverter));
        }

        public async Task<TValue> GetAsync(S3FileKey key, bool includeMetadata)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            return includeMetadata ? await GetWithMetadataAsync(key)
                                   : await GetWithoutMetadataAsync(key);
        }

        private async Task<TValue> GetWithMetadataAsync(S3FileKey key)
        {
            var getObjectsTask = GetObjectAsync(key);
            var getTagsTask = GetTagsAsync(key);
            await Task.WhenAll(getObjectsTask, getTagsTask);

            var result = new MemoryStream();
            IDictionary<string, string> metaData;
            using (var response = await getObjectsTask)
            {
                await response.ResponseStream.CopyToAsync(result);
                metaData = response.Metadata.ToDictionary();
            }
            result.Position = 0;

            var tags = await getTagsTask;

            return _containerConverter.GetDataContainer(result, Merge(metaData, tags));
        }

        private async Task<TValue> GetWithoutMetadataAsync(S3FileKey key)
        {
            var getObjectsTask = GetObjectAsync(key);

            var result = new MemoryStream();
            using (var response = await getObjectsTask)
            {
                await response.ResponseStream.CopyToAsync(result);
            }
            result.Position = 0;

            return _containerConverter.GetDataContainer(result, new Dictionary<string, string>());
        }

        private static Dictionary<string, string> Merge(IDictionary<string, string> metaData, IDictionary<string, string> tags)
        {
            return metaData
                .Concat(tags.ToList())
                .ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        private async Task<GetObjectResponse> GetObjectAsync(S3FileKey key)
        {
            var request = new GetObjectRequest
            {
                BucketName = BucketNameConstructor.GetBucketName(key),
                Key = key.Key,
                VersionId = key.VersionId
            };

            return await AmazonS3.GetObjectAsync(request);
        }

        private async Task<IDictionary<string, string>> GetTagsAsync(S3FileKey key)
        {
            var getTagsRequest = new GetObjectTaggingRequest
            {
                BucketName = BucketNameConstructor.GetBucketName(key),
                Key = key.Key,
                VersionId = key.VersionId
            };

            var objectTags = await AmazonS3.GetObjectTaggingAsync(getTagsRequest);

            return objectTags.Tagging.ToDictionary();
        }
    }
}