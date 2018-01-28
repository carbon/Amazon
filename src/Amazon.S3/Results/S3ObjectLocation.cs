using System;

namespace Amazon.S3
{
    public readonly struct S3ObjectLocation
    {
        public S3ObjectLocation(string bucketName, string key)
        {
            BucketName = bucketName ?? throw new ArgumentNullException(nameof(bucketName));
            Key        = key        ?? throw new ArgumentNullException(nameof(key));
        }

        public string BucketName { get; }

        public string Key { get; }
    }
}