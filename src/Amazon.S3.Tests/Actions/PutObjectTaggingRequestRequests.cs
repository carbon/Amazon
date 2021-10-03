namespace Amazon.S3.Actions.Tests;

public class PutObjectTaggingRequestRequests
{
    [Fact]
    public void Construct()
    {
        var request = new PutObjectTaggingRequest("s3.amazon.com", "bucket-name", "object-name", null, new[] {
            new KeyValuePair<string, string>("a", "1"),
            new KeyValuePair<string, string>("b", "2")
        });

        Assert.Equal(HttpMethod.Put, request.Method);
        Assert.Equal("/bucket-name/object-name?tagging", request.RequestUri.PathAndQuery);
        Assert.Equal(16, request.Content.Headers.ContentMD5.Length);
    }

    [Fact]
    public void ConstructWithVersion()
    {
        var request = new PutObjectTaggingRequest("s3.amazon.com", "bucket-name", "object-name", "1", new[] {
            new KeyValuePair<string, string>("a", "1"),
            new KeyValuePair<string, string>("b", "2")
        });

        Assert.Equal("/bucket-name/object-name?tagging&versionId=1", request.RequestUri.PathAndQuery);
    }
}