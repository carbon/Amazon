using System;
using System.Text;
using System.Net.Http;

namespace Amazon.S3
{
    public abstract class S3Request : HttpRequestMessage
    {
        public S3Request(HttpMethod method, AwsRegion region, string bucketName, string objectKey, string version = null)
        {
            #region Preconditions

            if (bucketName == null) throw new ArgumentNullException(nameof(bucketName));

            #endregion

            BucketName = bucketName;
            Key = objectKey;

            // https://{bucket}.s3.amazonaws.com/{key}

            // s3-external-1.amazonaws.com
            // storage.googleapis.com

            var urlBuilder = new StringBuilder();

            urlBuilder.Append("https://");
            urlBuilder.Append(bucketName);
            urlBuilder.Append(".");
            urlBuilder.Append(S3Host.Get(region));
            urlBuilder.Append("/");

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
    }

    internal static class S3Host
    {
        public static string Get(AwsRegion region)
        {
            switch (region.Name)
            {
                case "google"     : return "storage.googleapis.com";
                case "us-east-1"  : return "s3-external-1.amazonaws.com";
                default           : return $"s3-{region.Name}.amazonaws.com";
            }
        }
    }
}