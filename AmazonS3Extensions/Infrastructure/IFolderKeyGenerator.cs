using AmazonS3Extensions.Models;

namespace AmazonS3Extensions.Infrastructure
{
    public interface IFolderKeyGenerator
    {
        IFolderKeyGenerator WithFolderPath(string folderPath);
        IFolderKeyGenerator WithBucketType(BucketType bucketType);

        S3FolderKey Build();
    }
}
