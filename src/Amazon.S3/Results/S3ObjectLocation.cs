using System;

namespace Amazon.S3;

public readonly struct S3ObjectLocation
{
    public S3ObjectLocation(string bucketName, string key)
    {
        if (bucketName is null)
            throw new ArgumentNullException(nameof(bucketName));

        if (key is null)
            throw new ArgumentNullException(nameof(key));

        BucketName = bucketName;
        Key = key;
    }

    public string BucketName { get; }

    public string Key { get; }
}
