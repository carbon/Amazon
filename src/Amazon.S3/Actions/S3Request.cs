using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;

namespace Amazon.S3
{
    public abstract class S3Request : HttpRequestMessage
    {
        public S3Request(
            HttpMethod method,
            string host, 
            string bucketName, 
            string? objectName, 
            string? versionId = null)
        {
            if (host is null) throw new ArgumentNullException(nameof(host));

            BucketName = bucketName ?? throw new ArgumentNullException(nameof(bucketName));
            ObjectName = objectName;

            // https://{bucket}.s3.amazonaws.com/{key}

            var urlBuilder = new StringBuilder()
                .Append("https://")
                .Append(host)
                .Append('/')
                .Append(bucketName);

            // s3.dualstack.{region.Name}.amazonaws.com

            if (objectName != null)
            {
                urlBuilder.Append('/');
                urlBuilder.Append(objectName);
            }

            if (versionId != null && versionId.Length > 0)
            {
                if (versionId[0] == '?')
                {
                    urlBuilder.Append(versionId);
                }
                else
                {
                    urlBuilder.Append("?versionId=");
                    urlBuilder.Append(versionId);
                }
            }

            RequestUri = new Uri(urlBuilder.ToString());
            Method = method;
        }

        internal S3Request(
           HttpMethod method,
           string host,
           string bucketName,
           string? r,
           Dictionary<string, string> paramaters)
        {
            if (host is null) throw new ArgumentNullException(nameof(host));

            BucketName = bucketName ?? throw new ArgumentNullException(nameof(bucketName));

            using var urlBuilder = new StringWriter();

            urlBuilder.Write("https://");
            urlBuilder.Write(host);
            urlBuilder.Write('/');
            urlBuilder.Write(bucketName); 

            if (paramaters.Count > 0)
            {
                int i = 0;

                if (r != null)
                {
                    urlBuilder.Write('?');
                    urlBuilder.Write(r);
                    i++;
                }

                foreach (KeyValuePair<string, string> pair in paramaters)
                {
                    urlBuilder.Write(i == 0 ? '?' : '&');
                    urlBuilder.Write(pair.Key);

                    urlBuilder.Write('=');
                    UrlEncoder.Default.Encode(urlBuilder, pair.Value);

                    i++;
                }
            }

            RequestUri = new Uri(urlBuilder.ToString());
            Method = method;
        }

        public void SetStorageClass(StorageClass storageClass)
        {
            Headers.Add(S3HeaderNames.StorageClass, storageClass.Name);
        }

        public string BucketName { get; }

        public string? ObjectName { get; }

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