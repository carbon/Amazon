namespace Amazon.S3.Actions.Tests;

public class RequestOptionsTests
{
    [Fact]
    public void ListBucketOptions_RoundTripsValues()
    {
        var options = new ListBucketOptions {
            Delimiter         = "/",
            Prefix            = "a",
            ContinuationToken = "token",
            StartAfter        = "b",
            MaxKeys           = 5
        };

        Assert.Equal("/", options.Delimiter);
        Assert.Equal("a", options.Prefix);
        Assert.Equal("token", options.ContinuationToken);
        Assert.Equal("b", options.StartAfter);
        Assert.Equal(5, options.MaxKeys);

        var request = new ListBucketRequest("s3.test", "bucket", options);

        Assert.Equal("?list-type=2&delimiter=%2F&prefix=a&continuation-token=token&start-after=b&max-keys=5", request.RequestUri.Query);
    }

    [Fact]
    public void ListBucketOptions_NullRemovesValues()
    {
        var options = new ListBucketOptions {
            Prefix  = null,
            MaxKeys = null
        };

        Assert.Null(options.Prefix);
        Assert.Null(options.MaxKeys);

        var request = new ListBucketRequest("s3.test", "bucket", options);

        Assert.Equal("?list-type=2", request.RequestUri.Query);
    }

    [Fact]
    public void ListVersionsOptions_RoundTripsValues()
    {
        var options = new ListVersionsOptions {
            Delimiter       = "/",
            Prefix          = "a",
            VersionIdMarker = "v1",
            KeyMarker       = "k1",
            EncodingType    = "url",
            MaxKeys         = 5
        };

        Assert.Equal("/", options.Delimiter);
        Assert.Equal("a", options.Prefix);
        Assert.Equal("v1", options.VersionIdMarker);
        Assert.Equal("k1", options.KeyMarker);
        Assert.Equal("url", options.EncodingType);
        Assert.Equal(5, options.MaxKeys);
    }

    [Fact]
    public void ListVersionsOptions_NullRemovesValues()
    {
        var options = new ListVersionsOptions {
            Prefix  = null,
            MaxKeys = null
        };

        Assert.Null(options.Prefix);
        Assert.Null(options.MaxKeys);

        var request = new ListVersionsRequest("s3.test", "bucket", options);

        Assert.Equal("/bucket?versions", request.RequestUri.PathAndQuery);
    }
}
