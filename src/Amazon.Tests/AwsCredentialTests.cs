namespace Amazon.Security.Tests;

public class AwsCredentialTests
{
    [Fact]
    public void CanParse()
    {
        var accessKey = AwsCredential.Parse("a:b");

        Assert.Equal("a", accessKey.AccessKeyId);
        Assert.Equal("b", accessKey.SecretAccessKey);
        Assert.Null(accessKey.SecurityToken);
    }

    [Fact]
    public void Parse_KeepsColonsInSecret()
    {
        Assert.Equal("b:c", AwsCredential.Parse("a:b:c").SecretAccessKey);
    }

    [Fact]
    public void Parse_RejectsInvalidText()
    {
        Assert.Throws<ArgumentNullException>(() => AwsCredential.Parse(null));
        Assert.Throws<ArgumentException>(() => AwsCredential.Parse(""));
        Assert.Throws<ArgumentException>(() => AwsCredential.Parse("ab"));
    }

    [Fact]
    public void RequiresBothParts()
    {
        Assert.Throws<ArgumentNullException>(() => new AwsCredential(null, "b"));
        Assert.Throws<ArgumentNullException>(() => new AwsCredential("a", null));
    }

    [Fact]
    public async Task NeverRenews()
    {
        var credential = new AwsCredential("a", "b");

        Assert.False(credential.ShouldRenew);
        Assert.False(await credential.RenewAsync());
    }
}
