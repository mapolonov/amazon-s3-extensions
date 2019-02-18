using System;
using System.Linq;
using Amazon.S3.Model;

namespace AmazonS3Extensions.Extensions
{
    public static class S3ObjectExtensions
    {
        public static bool IsFolder(this S3Object obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            if (string.IsNullOrWhiteSpace(obj.Key)) throw new InvalidOperationException(nameof(obj.Key));

            return obj.Key.Last() == '/';
        }
    }
}
