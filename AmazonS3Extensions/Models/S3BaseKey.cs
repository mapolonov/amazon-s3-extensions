using System;

namespace AmazonS3Extensions.Models
{
    public abstract class S3BaseKey
    {
        protected S3BaseKey(string folderPath, BucketType bucketType)
        {
            if (bucketType == BucketType.None)
                throw new ArgumentException($"Value must not equal '{bucketType}'", nameof(bucketType));

            BucketType = bucketType;
            FolderPath = folderPath ?? throw new ArgumentNullException(nameof(folderPath));
        }

        public BucketType BucketType { get; }
        public string FolderPath { get; }
        public abstract string Key { get; }
    }
}
