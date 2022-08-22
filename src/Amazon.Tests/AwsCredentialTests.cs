namespace Amazon.Security.Tests;

public class AwsCredentialTests
{
    [Fact]
    public void CanParse()
    {
        var accessKey = AwsCredential.Parse("a:b");

        Assert.Equal("a", accessKey.AccessKeyId);
        Assert.Equal("b", accessKey.SecretAccessKey);
    }
}