using AmazonS3Extensions.Models;

namespace AmazonS3Extensions.Infrastructure
{
    public interface IFileKeyGenerator
    {
        IFileKeyGenerator WithFileName(string fileName);
        IFileKeyGenerator WithFolderPath(string folderPath);
        IFileKeyGenerator WithVersion(string version);
        IFileKeyGenerator WithBucketType(BucketType bucketType);

        S3FileKey Build();
    }
}
