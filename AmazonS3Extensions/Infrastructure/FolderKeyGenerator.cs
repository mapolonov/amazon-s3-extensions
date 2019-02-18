using AmazonS3Extensions.Models;

namespace AmazonS3Extensions.Infrastructure
{
    public class FolderKeyGenerator : IFolderKeyGenerator
    {
        private string _folderPath;
        private BucketType _bucketType;

        private FolderKeyGenerator()
        {
            _folderPath = string.Empty;
            _bucketType = BucketType.None;
        }

        public static IFolderKeyGenerator New()
            => new FolderKeyGenerator();

        public IFolderKeyGenerator WithFolderPath(string folderPath)
        {
            _folderPath = folderPath;
            return this;
        }

        public IFolderKeyGenerator WithBucketType(BucketType bucketType)
        {
            _bucketType = bucketType;
            return this;
        }

        public S3FolderKey Build()
        {
            return new S3FolderKey(_folderPath, _bucketType);
        }
    }
}
