using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Amazon.S3
{
    using Helpers;

    public class PutObjectRequest : S3Request
    {
        public PutObjectRequest(string host, string bucketName, string key)
            : base(HttpMethod.Put, host, bucketName, key)
        {
            CompletionOption = HttpCompletionOption.ResponseContentRead;
        }

        public void SetStream(Stream stream, string mediaType = "application/octet-stream")
        {
            #region Preconditions

            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            if (stream.Length == 0)
                throw new ArgumentException("Must not be empty", nameof(stream));

            if (mediaType == null)
                throw new ArgumentNullException(nameof(mediaType));

            #endregion

            Content = new StreamContent(stream);

            Content.Headers.ContentLength = stream.Length;
            Content.Headers.ContentType = new MediaTypeHeaderValue(mediaType);
            
            Headers.Add("x-amz-content-sha256", stream.CanSeek 
                ? HexString.FromBytes(ComputeSHA256(stream))
                : "UNSIGNED-PAYLOAD"
            );
        }

        internal void SetCustomerEncryptionKey(ServerSideEncryptionKey key)
        {
            Headers.Add("x-amz-server-side-encryption-customer-algorithm", key.Algorithm);
            Headers.Add("x-amz-server-side-encryption-customer-key",       Convert.ToBase64String(key.Key));
            Headers.Add("x-amz-server-side-encryption-customer-key-MD5",   Convert.ToBase64String(key.KeyMD5));
        }

        public void SetStream(Stream stream, long length, string mediaType = "application/octet-stream")
        {
            #region Preconditions

            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            if (length <= 0)
                throw new ArgumentException("Must be greater than 0.", nameof(length));

            #endregion
            
            Content = new StreamContent(stream);
            Content.Headers.ContentLength = length;
            Content.Headers.ContentType = new MediaTypeHeaderValue(mediaType);

            // TODO: Support chunked streaming...

            Headers.Add("x-amz-content-sha256",
                stream.CanSeek
                    ? HexString.FromBytes(ComputeSHA256(stream))
                    : "UNSIGNED-PAYLOAD");
        }
    }
}