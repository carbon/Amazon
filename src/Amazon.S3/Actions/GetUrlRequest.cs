using System;
using System.Text;

using Amazon.Security;

namespace Amazon.S3
{
    public sealed class GetPresignedUrlRequest
    {
        public GetPresignedUrlRequest(
            string method,
            string host,
            AwsRegion region,
            string bucketName,
            string objectKey,
            TimeSpan expiresIn)
        {
            Method = method ?? throw new ArgumentException(nameof(method));
            Host = host ?? throw new ArgumentNullException(nameof(host));
            Region = region;
            BucketName = bucketName ?? throw new ArgumentNullException(nameof(bucketName));
            Key = objectKey;
            ExpiresIn = expiresIn;
        }

        public string Method { get; }

        public string Host { get; }

        public string BucketName { get; }

        public AwsRegion Region { get; }

        public string Key { get; }

        public TimeSpan ExpiresIn { get; }

        internal string GetUrl()
        {
            int length = 10 + Host.Length + BucketName.Length + Key.Length;

            var sb = new StringBuilder(length);

            sb.Append("https://");
            sb.Append(Host);
            sb.Append('/');
            sb.Append(BucketName);
            sb.Append('/');
            sb.Append(Key);

            return sb.ToString();
        }
    }
}