﻿namespace Amazon.S3;

public readonly struct S3ObjectLocation
{
    public S3ObjectLocation(string bucketName!!, string key!!)
    {
        BucketName = bucketName;
        Key = key;
    }

    public string BucketName { get; }

    public string Key { get; }
}
