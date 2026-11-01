namespace Amazon.S3.Models.Tests;

public class UploadPartRequestTests
{
    [Fact]
    public void CanConstruct()
    {
        var request = new UploadPartRequest("s3.amazon.com", "bucket", "key", "uploadId", 1);

        Assert.Equal(new Uri("https://s3.amazon.com/bucket/key?partNumber=1&uploadId=uploadId"), request.RequestUri);
        Assert.Equal("uploadId", request.UploadId);
        Assert.Equal(1, request.PartNumber);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(10_001)]
    public void RejectsPartNumberOutOfRange(int partNumber)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new UploadPartRequest("s3.amazon.com", "bucket", "key", "uploadId", partNumber));
    }

    [Fact]
    public void RequiresUploadId()
    {
        Assert.Throws<ArgumentException>(() => new UploadPartRequest("s3.amazon.com", "bucket", "key", "", 1));
    }

    [Fact]
    public void ResultRequiresUploadIdAndETag()
    {
        Assert.Throws<ArgumentException>(() => new UploadPartResult("", 1, "eTag"));
        Assert.Throws<ArgumentException>(() => new UploadPartResult("uploadId", 1, ""));
    }
}