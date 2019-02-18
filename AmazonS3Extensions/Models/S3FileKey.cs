using System;
using System.IO;

namespace AmazonS3Extensions.Models
{
    public class S3FileKey : S3BaseKey
    {
        public override string ToString()
        {
            return $"BucketType: {BucketType}, FileKey: {Key}, Version: {VersionId}";
        }

        public S3FileKey(
            string fileName,
            BucketType bucketType) : this(string.Empty, fileName, bucketType)
        {
        }

        public S3FileKey(
            string folderPath,
            string fileName,
            BucketType bucketType,
            string versionId = null) : base(folderPath, bucketType)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(fileName));
            if (versionId != null && versionId.Trim() == string.Empty)
                throw new ArgumentException("Value cannot be empty or whitespace.", nameof(versionId));

            FileName = fileName;
            VersionId = versionId;
        }

        public string FileName { get; }
        public string VersionId { get; }
        public override string Key => Path.Combine(FolderPath, FileName).Replace("\\", "/");
    }
}
