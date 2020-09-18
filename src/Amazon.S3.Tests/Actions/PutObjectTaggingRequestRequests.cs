using System;
using System.Collections.Generic;
using System.Net.Http;

using Xunit;

namespace Amazon.S3.Actions.Tests
{
    public class PutObjectTaggingRequestRequests
    {
        [Fact]
        public void Construct()
        {
            var request = new PutObjectTaggingRequest("s3.amazon.com", "bucket-name", "object-name", null, new[] {
                new KeyValuePair<string, string>("a", "1"),
                new KeyValuePair<string, string>("b", "2")
            });

            Assert.Equal(HttpMethod.Put, request.Method);
            Assert.Equal("Kke0ftIhilXTF63g5XZL2g==", Convert.ToBase64String(request.Content.Headers.ContentMD5));

            Assert.Equal("/bucket-name/object-name?tagging", request.RequestUri.PathAndQuery);
        }

        [Fact]
        public void ConstructWithVersion()
        {
            var request = new PutObjectTaggingRequest("s3.amazon.com", "bucket-name", "object-name", "1", new[] {
                new KeyValuePair<string, string>("a", "1"),
                new KeyValuePair<string, string>("b", "2")
            });

            Assert.Equal("/bucket-name/object-name?tagging&versionId=1", request.RequestUri.PathAndQuery);
        }
    }
}