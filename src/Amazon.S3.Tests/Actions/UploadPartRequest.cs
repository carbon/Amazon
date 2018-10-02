using System;
using Xunit;

namespace Amazon.S3.Models.Tests
{
    public class UploadPartRequestTests
    {
        [Fact]
        public void Construct()
        {
            var a = new UploadPartRequest("s3.amazon.com", "bucket", "key", "uploadId", 1);

            Assert.Equal(new Uri("https://s3.amazon.com/bucket/key?partNumber=1&uploadId=uploadId"), a.RequestUri);

            Assert.Equal("uploadId", a.UploadId);
            Assert.Equal(1, a.PartNumber);
        }
    }
}