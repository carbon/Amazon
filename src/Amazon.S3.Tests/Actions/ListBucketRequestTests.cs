namespace Amazon.S3.Actions.Tests;

public class ListBucketRequestTests
{
    [Fact]
    public void Construct()
    {
        var request = new ListBucketRequest("s3.amazon.com", "bucket", new ListBucketOptions { MaxKeys = 1000 });

        Assert.Equal("https://s3.amazon.com/bucket?list-type=2&max-keys=1000", request.RequestUri.ToString());
    }
}