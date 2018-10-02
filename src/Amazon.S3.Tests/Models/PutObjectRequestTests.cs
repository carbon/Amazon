using System;
using System.IO;
using System.Linq;
using Xunit;

namespace Amazon.S3.Tests
{
    public class PutObjectRequestTests
    {
        [Fact]
        public void Construct()
        {
            var request = new PutObjectRequest("s3.amazon.com", "bucket", "key");

            request.SetStorageClass(StorageClass.ReducedRedundancy);

            var ms = new MemoryStream();

            ms.WriteByte(1);

            request.SetStream(ms, "image/jpeg");

            Assert.Equal("image/jpeg", string.Join(';', request.Content.Headers.GetValues("Content-Type")));
            Assert.Equal(new Uri("https://s3.amazon.com/bucket/key"), request.RequestUri);
            Assert.Equal("REDUCED_REDUNDANCY", request.Headers.GetValues("x-amz-storage-class").First());
        }
    }
}
