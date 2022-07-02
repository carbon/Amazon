namespace Amazon.S3.Actions.Tests;

public class DeleteObjectBatchRequestTests
{
    [Fact]
    public void CanConstruct()
    {
        var batch = new DeleteBatch(new[] { "a", "b" });

        var request = new DeleteObjectsRequest("s3.amazon.com", "bucket", batch);

        Assert.Equal("https://s3.amazon.com/bucket?delete", request.RequestUri.ToString());
        Assert.Equal(HttpMethod.Post, request.Method);

        Assert.Equal("gnFQWv8HmHVEQ7mTrck6JQ==", Convert.ToBase64String(request.Content.Headers.ContentMD5));
    }
}
