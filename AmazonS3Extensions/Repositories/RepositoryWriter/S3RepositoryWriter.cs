using System;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using AmazonS3Extensions.Contract;
using AmazonS3Extensions.Extensions;
using AmazonS3Extensions.Infrastructure;
using AmazonS3Extensions.Models;

namespace AmazonS3Extensions.Repositories.RepositoryWriter
{
    public class S3RepositoryWriter<TValue> : S3RepositoryBase, IRepositoryWriter<S3FileKey, TValue>
    {
        private readonly IDataContainerConverter<TValue> _containerConverter;

        public S3RepositoryWriter(IAmazonS3 amazonS3,
            IBucketNameConstructor bucketNameConstructor,
            IDataContainerConverter<TValue> containerConverter) : base(bucketNameConstructor, amazonS3)
        {
            _containerConverter = containerConverter ?? throw new ArgumentNullException(nameof(containerConverter));
        }

        public async Task PostAsync(S3FileKey key, TValue source)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (source == null) throw new ArgumentNullException(nameof(source));

            var stream = _containerConverter.GetStream(source);
            var metaData = _containerConverter.GetMetaData(source);
            stream.Position = 0;
            var request = new PutObjectRequest
            {
                BucketName = BucketNameConstructor.GetBucketName(key),
                Key = key.Key,
                InputStream = stream,
                TagSet = metaData.ToTagSet(_containerConverter.GetOriginKey()).ToList(),
                AutoCloseStream = false
            };

            metaData.CopyToMetadataCollection(request.Metadata, _containerConverter.GetOriginKey());
            await AmazonS3.PutObjectAsync(request);

            stream.Position = 0;
        }
    }
}