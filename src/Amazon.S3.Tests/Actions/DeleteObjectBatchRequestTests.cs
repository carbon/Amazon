namespace Amazon.S3.Actions.Tests;

public class DeleteObjectBatchRequestTests
{
    [Fact]
    public void CanConstruct()
    {
        var batch = new DeleteBatch([ "a", "b" ]);

        var request = new DeleteObjectsRequest("s3.amazon.com", "bucket", batch);

        Assert.Equal("https://s3.amazon.com/bucket?delete", request.RequestUri.ToString());
        Assert.Equal(HttpMethod.Post, request.Method);

        Assert.Equal("gnFQWv8HmHVEQ7mTrck6JQ==", Convert.ToBase64String(request.Content.Headers.ContentMD5));
    }

    [Fact]
    public async Task EscapesKeys()
    {
        const string key = "a&b<c>d\re\"f'g";

        var request = new DeleteObjectsRequest("s3.amazon.com", "bucket", new DeleteBatch([ key ]));

        byte[] data = await request.Content.ReadAsByteArrayAsync();

        Assert.Equal("<Delete><Object><Key>a&amp;b&lt;c&gt;d&#xD;e\"f'g</Key></Object></Delete>"u8, data);

        using var stream = new MemoryStream(data);

        Assert.Equal(key, System.Xml.Linq.XDocument.Load(stream).Root.Element("Object").Element("Key").Value);
    }

    [Fact]
    public async Task ContentMatchesXml()
    {
        var batch = new DeleteBatch([ "é", "b" ], quite: true);

        var request = new DeleteObjectsRequest("s3.amazon.com", "bucket", batch);

        byte[] data = await request.Content.ReadAsByteArrayAsync();

        Assert.Equal("<Delete><Quiet>true</Quiet><Object><Key>é</Key></Object><Object><Key>b</Key></Object></Delete>"u8, data);
    }
}
