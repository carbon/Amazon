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
    public void SetStreamWithLengthSetsSha256Header()
    {
        var request = new UploadPartRequest("s3.amazon.com", "bucket", "key", "uploadId", 1);

        using var stream = new System.IO.MemoryStream("abc"u8.ToArray());

        request.SetStream(stream, length: 3);

        Assert.Equal(
            "ba7816bf8f01cfea414140de5dae2223b00361a396177a9cb410ff61f20015ad",
            Assert.Single(request.Headers.GetValues("x-amz-content-sha256")));

        Assert.Equal(0, stream.Position);
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