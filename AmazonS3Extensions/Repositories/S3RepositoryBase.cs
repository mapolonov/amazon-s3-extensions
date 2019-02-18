using System;
using Amazon.S3;
using AmazonS3Extensions.Infrastructure;

namespace AmazonS3Extensions.Repositories
{
    public abstract class S3RepositoryBase : IDisposable
    {
        protected readonly IBucketNameConstructor BucketNameConstructor;
        protected readonly IAmazonS3 AmazonS3;

        protected S3RepositoryBase(IBucketNameConstructor bucketNameConstructor,
            IAmazonS3 amazonS3)
        {
            BucketNameConstructor = bucketNameConstructor?? throw new ArgumentNullException(nameof(bucketNameConstructor));
            AmazonS3 = amazonS3 ?? throw new ArgumentNullException(nameof(amazonS3));
        }

        #region IDisposable Support
        private bool _disposedValue; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (_disposedValue) return;

            if (disposing)
            {
                AmazonS3.Dispose();
            }

            _disposedValue = true;
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}