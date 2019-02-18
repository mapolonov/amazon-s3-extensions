using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using AmazonS3Extensions.Contract;
using AmazonS3Extensions.Extensions;
using AmazonS3Extensions.Infrastructure;
using AmazonS3Extensions.Models;

namespace AmazonS3Extensions.Repositories.BatchRepositoryReader
{
    public class S3BatchRepositoryReader : S3RepositoryBase, IBatchRepositoryReader<S3FileKey, S3FolderKey>
    {
        public S3BatchRepositoryReader(IBucketNameConstructor bucketNameConstructor,
            IAmazonS3 amazonS3) : base(bucketNameConstructor, amazonS3)
        {
        }

        public async Task<IEnumerable<S3FileKey>> GetAllKeysAsync(S3FolderKey filter)
        {
            if (filter == null) throw new ArgumentNullException(nameof(filter));

            var result = new List<S3FileKey>();
            var request = CreateRequest(filter);

            ListObjectsV2Response response;
            do
            {
                response = await AmazonS3.ListObjectsV2Async(request);

                var keys = ProcessResponse(response, filter);
                result.AddRange(keys);

                request.ContinuationToken = response.NextContinuationToken;
            } while (response.IsTruncated);

            return result;
        }

        private IEnumerable<S3FileKey> ProcessResponse(ListObjectsV2Response response, S3FolderKey filter)
        {
            if (response == null) throw new ArgumentNullException(nameof(response));

            foreach (var s3Object in response.S3Objects.Where(x => !x.IsFolder()))
                yield return FileKeyGenerator.New().WithFileName(s3Object.Key)
                                               .WithBucketType(filter.BucketType)
                                               .Build();
        }

        private ListObjectsV2Request CreateRequest(S3FolderKey filter)
        {
            var request = new ListObjectsV2Request
            {
                BucketName = BucketNameConstructor.GetBucketName(filter),
                Prefix = filter.Key,
                Delimiter = "/"
            };

            return request;
        }
    }
}
