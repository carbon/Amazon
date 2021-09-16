using System;
using System.Net;
using System.Net.Http;
using System.Text;

using Xunit;

namespace Amazon.S3.Tests
{
    public class S3ObjectTests
    {
        [Fact]
        public void CanConstructFromHttpResponse()
        {
            var response = GetMockResponse();

            using var blob = new S3Object("key", response);

            Assert.Equal(5, blob.Properties.Count);

            Assert.Equal(7, blob.ContentLength);
            Assert.Null(blob.ContentRange);

            Assert.Equal("Wed, 01 Jan 2020 01:01:01 GMT", blob.Properties["Last-Modified"]);

            Assert.Equal(new DateTimeOffset(2020, 01, 01, 01, 01, 01, TimeSpan.Zero), blob.LastModified);
            Assert.Equal("application/json; charset=utf-8", blob.ContentType);

            Assert.Same(blob.ContentType, blob.Properties["Content-Type"]);
        }

        [Fact]
        public void CanDispose()
        {
            var response = GetMockResponse();

            var blob = new S3Object("key", response);

            blob.Dispose();

            Assert.ThrowsAsync<ObjectDisposedException>(async () => await blob.OpenAsync());
        }


        private static HttpResponseMessage GetMockResponse()
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);

            var lastModified = new DateTimeOffset(2020, 01, 01, 01, 01, 01, TimeSpan.Zero);

            response.Headers.TryAddWithoutValidation("x-amz-meta-format", "json");

            response.Content = new StringContent("[1,2,3]", Encoding.UTF8, "application/json")
            {
                Headers = {
                    { "Content-Length", "7" },
                    { "Last-Modified", lastModified.ToString("R") },
                    { "x-test", "1" }
                }
            };

            return response;
        }
    }
}
