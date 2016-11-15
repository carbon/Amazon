using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Amazon.S3
{
    public class PutObjectRequest : S3Request
    {
        public PutObjectRequest(AwsRegion region, string bucketName, string key)
            : base(HttpMethod.Put, region, bucketName, key)
        {
            this.CompletionOption = HttpCompletionOption.ResponseContentRead;
        }

        public void SetStream(Stream stream, string mediaType = "application/octet-stream")
        {
            #region Preconditions

            if (stream == null)
                throw new ArgumentNullException("stream");

            if (stream.Length <= 0)
                throw new ArgumentException("Must be greater than 0. Key: " + Key, "stream.Length");

            #endregion

            this.Content = new StreamContent(stream);

            this.Content.Headers.ContentLength = stream.Length;
            this.Content.Headers.ContentType = new MediaTypeHeaderValue(mediaType);
        }

        public void SetStream(Stream stream, int length, string mediaType = "application/octet-stream")
        {
            #region Preconditions

            if (stream == null) throw new ArgumentNullException("stream");
            if (length <= 0) throw new ArgumentException("Must be greater than 0.", "length");

            #endregion

            this.Content = new StreamContent(stream);

            this.Content.Headers.ContentLength = length;
            this.Content.Headers.ContentType = new MediaTypeHeaderValue(mediaType);
        }
    }
}