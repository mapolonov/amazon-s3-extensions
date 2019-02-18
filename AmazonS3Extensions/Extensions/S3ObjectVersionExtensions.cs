using System;
using Amazon.S3.Model;
using AmazonS3Extensions.Infrastructure;
using AmazonS3Extensions.Models;

namespace AmazonS3Extensions.Extensions
{
    public static class S3ObjectVersionExtensions
    {
        public static S3FileKey ToVersionedS3FileKey(this S3ObjectVersion objectVersion, S3FileKey initialKey)
        {
            if (objectVersion == null) throw new ArgumentNullException(nameof(objectVersion));
            if (initialKey == null) throw new ArgumentNullException(nameof(initialKey));
            
            return FileKeyGenerator.New()
                .WithFileName(initialKey.FileName)
                .WithFolderPath(initialKey.FolderPath)
                .WithBucketType(initialKey.BucketType)
                .WithVersion(objectVersion.VersionId)
                .Build();
        }
    }
}