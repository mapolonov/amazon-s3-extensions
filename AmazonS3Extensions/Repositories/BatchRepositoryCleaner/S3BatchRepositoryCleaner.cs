using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using AmazonS3Extensions.Common;
using AmazonS3Extensions.Contract;
using AmazonS3Extensions.Infrastructure;
using AmazonS3Extensions.Models;

namespace AmazonS3Extensions.Repositories.BatchRepositoryCleaner
{
    public class S3BatchRepositoryCleaner : S3RepositoryBase, IBatchRepositoryCleaner<S3FolderKey>
    {
        private const int FilesLimitPerRequest = 1000;
        private const int MaxConcurrentTasks = 3;
        private readonly IBatchRepositoryReader<S3FileKey, S3FolderKey> _batchRepositoryReader;

        public S3BatchRepositoryCleaner(
            IBatchRepositoryReader<S3FileKey, S3FolderKey> batchRepositoryReader,
            IBucketNameConstructor bucketNameConstructor,
            IAmazonS3 amazonS3) : base(bucketNameConstructor, amazonS3)
        {
            _batchRepositoryReader = batchRepositoryReader ?? throw new ArgumentNullException(nameof(batchRepositoryReader));
        }

        public async Task DeleteAsync(S3FolderKey folderKey)
        {
            if(folderKey == null) throw new ArgumentNullException(nameof(folderKey));
            var fileKeys = (await _batchRepositoryReader.GetAllKeysAsync(folderKey)).ToList();

            while (fileKeys.Any())
            {
                var requests = CreateRequests(fileKeys, folderKey);

                var tasks = requests.Select(request => AmazonS3.DeleteObjectsAsync(request));
                await tasks.ThrottleAsync(MaxConcurrentTasks);

                //request for keys again, because sometimes some files are still not deleted
                fileKeys = (await _batchRepositoryReader.GetAllKeysAsync(folderKey)).ToList(); 
            }
        }

        private IEnumerable<DeleteObjectsRequest> CreateRequests(IList<S3FileKey> fileKeys, S3FolderKey folderKey)
        {
            var requests = new List<DeleteObjectsRequest>();

            int requestCount = fileKeys.Count / FilesLimitPerRequest + 1;
            for (var i = 0; i < requestCount; i++)
            {
                var request = new DeleteObjectsRequest
                {
                    BucketName = BucketNameConstructor.GetBucketName(folderKey),
                    Objects = fileKeys.Skip(i * FilesLimitPerRequest)
                                     .Take(FilesLimitPerRequest)
                                     .Select(k => new KeyVersion
                                        {
                                            Key = k.Key
                                        }).ToList()
                };

                requests.Add(request);
            }

            return requests;
        }
    }
}
