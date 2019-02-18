using AmazonS3Extensions.Models;

namespace AmazonS3Extensions.Infrastructure
{
    public class FileKeyGenerator : IFileKeyGenerator
    {
        private string _fileName;
        private string _folderPath;
        private string _version;
        private BucketType _bucketType;

        private FileKeyGenerator()
        {
            _fileName = string.Empty;
            _folderPath = string.Empty;
            _bucketType = BucketType.None;
        }

        public static IFileKeyGenerator New()
            => new FileKeyGenerator();

        public IFileKeyGenerator WithFileName(string fileName)
        {
            _fileName = fileName;
            return this;
        }

        public IFileKeyGenerator WithFolderPath(string folderPath)
        {
            _folderPath = folderPath;
            return this;
        }

        public IFileKeyGenerator WithVersion(string version)
        {
            _version = version;
            return this;
        }

        public IFileKeyGenerator WithBucketType(BucketType bucketType)
        {
            _bucketType = bucketType;
            return this;
        }

        public S3FileKey Build()
        {
            return new S3FileKey(_folderPath, _fileName, _bucketType, _version);
        }
    }
}
