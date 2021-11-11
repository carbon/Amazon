namespace Amazon.S3.Models.Tests;

public class UploadPartRequestTests
{
    [Fact]
    public void Construct()
    {
        var request = new UploadPartRequest("s3.amazon.com", "bucket", "key", "uploadId", 1);

        Assert.Equal(new Uri("https://s3.amazon.com/bucket/key?partNumber=1&uploadId=uploadId"), request.RequestUri);
        Assert.Equal("uploadId", request.UploadId);
        Assert.Equal(1, request.PartNumber);
    }
}