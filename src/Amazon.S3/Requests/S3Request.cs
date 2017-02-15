using System;
using System.Text;
using System.Net.Http;
using System.IO;
using System.Security.Cryptography;

namespace Amazon.S3
{
    public abstract class S3Request : HttpRequestMessage
    {
        public S3Request(HttpMethod method, AwsRegion region, string bucketName, string objectKey, string version = null)
        {
            #region Preconditions

            if (region == null)
                throw new ArgumentNullException(nameof(region));

            if (bucketName == null)
                throw new ArgumentNullException(nameof(bucketName));

            #endregion

            BucketName = bucketName;
            Key = objectKey;

            // https://{bucket}.s3.amazonaws.com/{key}

            var urlBuilder = new StringBuilder()
                .Append("https://")
                .Append(bucketName)
                .Append(".")
                .Append(S3Host.Get(region))
                .Append("/");

            if (objectKey != null)
            {
                urlBuilder.Append(objectKey);
            }

            if (version != null)
            {
                urlBuilder.Append("?version=");
                urlBuilder.Append(version);
            }

            var url = urlBuilder.ToString();

            RequestUri = new Uri(url);
            Method = method;
            CompletionOption = HttpCompletionOption.ResponseHeadersRead;
        }

        public void SetStorageClass(StorageClass storageClass)
        {
            Headers.Add("x-amz-storage-class", storageClass.ToString());
        }

        public string BucketName { get; }

        public string Key { get; }

        public HttpCompletionOption CompletionOption { get; set; }

        #region Helpers


        protected byte[] ComputeSHA256(Stream stream)
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

    internal static class S3Host
    {
        public static string Get(AwsRegion region)
        {
            if (region.Name == "google")
            {
                return "storage.googleapis.com";
            }
          
            // Use dualstack to support IP6 in the future
            return $"s3.dualstack.{region.Name}.amazonaws.com";
        }
    }
}