namespace AmazonS3Extensions.Models
{
    public class S3FolderKey : S3BaseKey
    {
        public S3FolderKey(string folderPath, BucketType bucketType) : base(folderPath, bucketType)
        {
        }

        public override string ToString()
        {
            return $"BucketType: {BucketType}, FolderKey: {Key}";
        }

        public override string Key => FolderPath;
    }
}