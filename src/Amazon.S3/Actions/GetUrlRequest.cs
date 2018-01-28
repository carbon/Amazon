using System;

namespace Amazon.S3
{    
    public readonly struct GetPresignedUrlRequest
    {
        public GetPresignedUrlRequest(
            string host,
            AwsRegion region, 
            string bucketName, 
            string objectKey, 
            TimeSpan expiresIn)
        {
            Host       = host  ?? throw new ArgumentNullException(nameof(host));
            Region     = region;
            BucketName = bucketName ?? throw new ArgumentNullException(nameof(bucketName));
            Key        = objectKey;
            ExpiresIn  = expiresIn;
        }

        public readonly string Host;

        public readonly string BucketName;

        public readonly AwsRegion Region;

        public readonly string Key;

        public readonly TimeSpan ExpiresIn;
    }
}
