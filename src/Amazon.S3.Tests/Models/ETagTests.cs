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

    [Theory]
    [InlineData("\"1f8ada2ce841b291cfcd6b9b4b645044-2\"")]
    [InlineData(null)]
    public void AsMD5_IsNullWhenNotAnMD5(string value)
    {
        Assert.Null(new ETag(value).AsMD5());
    }

    [Fact]
    public void ConvertsToString()
    {
        string value = new ETag("\"abc\"");

        Assert.Equal("\"abc\"", value);
    }
}