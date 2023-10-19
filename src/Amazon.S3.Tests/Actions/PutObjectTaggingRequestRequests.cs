using System.Text;

namespace Amazon.S3.Actions.Tests;

public class PutObjectTaggingRequestRequests
{
    [Fact]
    public void CanConstruct()
    {
        var request = new PutObjectTaggingRequest("s3.amazon.com", "bucket-name", "object-name", null, [
            new("a", "1"),
            new("b", "2")
        ]);

        Assert.Equal(HttpMethod.Put, request.Method);
        Assert.Equal("/bucket-name/object-name?tagging", request.RequestUri.PathAndQuery);
        Assert.Equal(16, request.Content.Headers.ContentMD5.Length);
    }

    [Fact]
    public async Task CanConstructWithVersion()
    {
        var request = new PutObjectTaggingRequest("s3.amazon.com", "bucket-name", "object-name", "1", [
            new("a", "1"),
            new("b", "2")
        ]);

        Assert.Equal("/bucket-name/object-name?tagging&versionId=1", request.RequestUri.PathAndQuery);

        var data = await request.Content.ReadAsByteArrayAsync();

        Assert.Equal("<Tagging><TagSet><Tag><Key>a</Key><Value>1</Value></Tag><Tag><Key>b</Key><Value>2</Value></Tag></TagSet></Tagging>", Encoding.UTF8.GetString(data));
    }
}