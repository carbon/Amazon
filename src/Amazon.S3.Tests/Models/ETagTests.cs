namespace Amazon.S3.Models.Tests;

public class ETagTests
{
    [Fact]
    public void Parse()
    {
        var eTag = new ETag("\"5d41402abc4b2a76b9719d911017c592\"");

        Assert.Equal(16, eTag.AsMD5().Length);

        Assert.Equal("5d41402abc4b2a76b9719d911017c592", Convert.ToHexStringLower(eTag.AsMD5()));
    }
}