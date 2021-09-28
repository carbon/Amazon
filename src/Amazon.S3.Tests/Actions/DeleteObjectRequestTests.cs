using System.Net.Http;

namespace Amazon.S3.Actions.Tests;

public class DeleteObjectRequestTests
{
    [Fact]
    public void Construct()
    {
        var request = new DeleteObjectRequest("s3.amazon.com", "bucket", "key", "1");

        Assert.Equal("https://s3.amazon.com/bucket/key?versionId=1", request.RequestUri.ToString());
        Assert.Equal(HttpMethod.Delete, request.Method);
    }
}