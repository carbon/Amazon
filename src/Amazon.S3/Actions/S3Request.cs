using System;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;

namespace Amazon.S3
{
    public abstract class S3Request : HttpRequestMessage
    {
        public S3Request(
            HttpMethod method,
            string host, 
            string bucketName, 
            string objectName, 
            string version = null)
        {
            if (host is null) throw new ArgumentNullException(nameof(host));

            BucketName = bucketName ?? throw new ArgumentNullException(nameof(bucketName));
            ObjectName = objectName;

            // https://{bucket}.s3.amazonaws.com/{key}

            var urlBuilder = StringBuilderCache.Aquire()
                .Append("https://")
                .Append(host)
                .Append('/')
                .Append(bucketName)
                .Append('/');

            // s3.dualstack.{region.Name}.amazonaws.com

            if (objectName != null)
            {
                urlBuilder.Append(objectName);
            }

            if (version != null)
            {
                urlBuilder.Append("?version=");
                urlBuilder.Append(version);
            }

            RequestUri = new Uri(StringBuilderCache.ExtractAndRelease(urlBuilder));
            Method = method;
        }

        public void SetStorageClass(StorageClass storageClass)
        {
            Headers.Add("x-amz-storage-class", storageClass.Name);
        }

        public string BucketName { get; }

        public string ObjectName { get; }

        public HttpCompletionOption CompletionOption { get; set; } = HttpCompletionOption.ResponseHeadersRead;

        #region Helpers

        protected static byte[] ComputeSHA256(Stream stream)
        {
            using SHA256 sha = SHA256.Create();

            byte[] data = sha.ComputeHash(stream);

            stream.Position = 0;

            return data;
        }

        #endregion
    }
}