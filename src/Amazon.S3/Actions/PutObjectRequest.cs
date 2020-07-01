using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

using Amazon.Helpers;

namespace Amazon.S3
{
    public class PutObjectRequest : S3Request
    {
        public PutObjectRequest(string host, string bucketName, string key)
            : base(HttpMethod.Put, host, bucketName, key)
        {
            if (key is null) throw new ArgumentNullException(nameof(key));

            CompletionOption = HttpCompletionOption.ResponseContentRead;
        }

        public void SetStream(Stream stream, string mediaType = "application/octet-stream")
        {
            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            if (stream.Length == 0)
                throw new ArgumentException("Must not be empty", nameof(stream));

            if (mediaType is null)
                throw new ArgumentNullException(nameof(mediaType));

            if (mediaType.Length == 0)
                throw new ArgumentException(nameof(mediaType), "Required");

            Content = new StreamContent(stream);

            Content.Headers.ContentLength = stream.Length;

            Content.Headers.Add("Content-Type", mediaType);

            Headers.Add("x-amz-content-sha256", stream.CanSeek 
                ? HexString.FromBytes(ComputeSHA256(stream))
                : "UNSIGNED-PAYLOAD"
            );
        }

        internal void SetCustomerEncryptionKey(in ServerSideEncryptionKey key)
        {
            Headers.Add(S3HeaderNames.ServerSideEncryptionCustomerAlgorithm, key.Algorithm);
            Headers.Add(S3HeaderNames.ServerSideEncryptionCustomerKey,       Convert.ToBase64String(key.Key));
            Headers.Add(S3HeaderNames.ServerSideEncryptionCustomerKeyMD5,    Convert.ToBase64String(key.KeyMD5));
        }

        public void SetStream(Stream stream, long length, string mediaType = "application/octet-stream")
        {
            if (stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if (length <= 0)
            {
                throw new ArgumentException("Must be greater than 0.", nameof(length));
            }

            Content = new StreamContent(stream);
            Content.Headers.ContentLength = length;
            Content.Headers.ContentType = new MediaTypeHeaderValue(mediaType);

            // TODO: Support chunked streaming...

            Headers.Add("x-amz-content-sha256", stream.CanSeek
                ? HexString.FromBytes(ComputeSHA256(stream))
                : "UNSIGNED-PAYLOAD");
        }
    }
}