namespace Amazon.S3.Actions.Tests;

public class GetObjectRequestTests
{
    [Fact]
    public void SetRange()
    {
        var request = new GetObjectRequest("s3.amazon.com", "bucket", "objectKey");

        request.SetRange(int.MaxValue, 9147484647);
        
        Assert.Equal("bytes=2147483647-9147484647", request.Headers.NonValidated["Range"].ToString());
    }

    [Fact]
    public void SetRange2()
    {
        var request = new GetObjectRequest("s3.amazon.com", "bucket", "objectKey");

        request.SetRange(1325400064, 1342177279);
        Assert.Equal("bytes=1325400064-1342177279", request.Headers.NonValidated["Range"].ToString());
    }

    [Fact]
    public void RequiresKey()
    {
        Assert.Throws<ArgumentNullException>(() => new GetObjectRequest("s3.amazon.com", "bucket", null));
    }

    [Fact]
    public void IfModifiedSince()
    {
        var date = new DateTimeOffset(2020, 01, 01, 01, 01, 01, TimeSpan.Zero);

        var request = new GetObjectRequest("s3.amazon.com", "bucket", "objectKey") {
            IfModifiedSince = date
        };

        Assert.Equal(date, request.IfModifiedSince);
        Assert.Equal("Wed, 01 Jan 2020 01:01:01 GMT", request.Headers.NonValidated["If-Modified-Since"].ToString());
    }

    [Fact]
    public void IfNoneMatch()
    {
        var request = new GetObjectRequest("s3.amazon.com", "bucket", "objectKey") {
            IfNoneMatch = "\"abc\""
        };

        Assert.Equal("\"abc\"", request.Headers.NonValidated["If-None-Match"].ToString());

        request.IfNoneMatch = null;

        Assert.Empty(request.Headers.IfNoneMatch);
    }
}
