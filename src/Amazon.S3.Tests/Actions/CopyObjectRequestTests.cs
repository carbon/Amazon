using System;
using System.Linq;

using Xunit;

namespace Amazon.S3.Actions.Tests
{
    public class CopyObjectRequestTests
    {
        [Fact]
        public void CanConstruct()
        {
            var request = new CopyObjectRequest(
                host: "s3.amazon.com",
                source: new S3ObjectLocation("sourceBucket", "video.mp4"),
                target: new S3ObjectLocation("targetBucket", "video.mp4"));


            Assert.Equal(new Uri("https://s3.amazon.com/targetBucket/video.mp4"), request.RequestUri);
            Assert.Null(request.MetadataDirective);
            Assert.False(request.Headers.Contains("x-amz-metadata-directive"));
            Assert.Equal("/sourceBucket/video.mp4", request.Headers.GetValues("x-amz-copy-source").FirstOrDefault());
        }

        [Fact]
        public void CanSetMetdataDirective()
        {
            var request = new CopyObjectRequest(
                host   : "s3.amazon.com",
                source : new S3ObjectLocation("sourceBucket", "video.mp4"),
                target : new S3ObjectLocation("targetBucket", "video.mp4")
            );

            request.MetadataDirective = MetadataDirectiveValue.Replace;

            Assert.Equal("REPLACE", request.Headers.GetValues("x-amz-metadata-directive").First());

            Assert.Equal(MetadataDirectiveValue.Replace, request.MetadataDirective);

            request.MetadataDirective = null;

            Assert.Null(request.MetadataDirective);
        }
    }
}