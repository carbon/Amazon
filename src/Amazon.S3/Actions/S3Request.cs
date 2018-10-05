using System;
using System.Text;
using System.Net.Http;
using System.IO;
using System.Security.Cryptography;

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

            var urlBuilder = new StringBuilder()
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

            RequestUri = new Uri(urlBuilder.ToString());
            Method = method;
            CompletionOption = HttpCompletionOption.ResponseHeadersRead;
        }

        public void SetStorageClass(StorageClass storageClass)
        {
            Headers.Add("x-amz-storage-class", storageClass.Name);
        }

        public string BucketName { get; }

        public string ObjectName { get; }

        public HttpCompletionOption CompletionOption { get; set; }

        #region Helpers

        protected static byte[] ComputeSHA256(Stream stream)
        {
            using (var sha = SHA256.Create())
            {
                var data = sha.ComputeHash(stream);

                stream.Position = 0;

                return data;
            }
        }

        #endregion
    }
}