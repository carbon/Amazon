using System;
using System.IO;
using System.Linq;

namespace Amazon.S3.Tests;

public class PutObjectRequestTests
{
    [Fact]
    public void Construct()
    {
        var request = new PutObjectRequest("s3.amazon.com", "bucket", "key");

        request.SetStorageClass(StorageClass.ReducedRedundancy);

        var ms = new MemoryStream(1);

        ms.WriteByte(1);

        request.SetStream(ms, "image/jpeg");

        var sha256 = request.Headers.GetValues("x-amz-content-sha256").First();

        Assert.Equal("image/jpeg", string.Join(';', request.Content.Headers.GetValues("Content-Type")));
        Assert.Equal(new Uri("https://s3.amazon.com/bucket/key"), request.RequestUri);
        Assert.Equal("REDUCED_REDUNDANCY", request.Headers.GetValues("x-amz-storage-class").First());
        Assert.Equal("e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855", sha256);
    }
}
