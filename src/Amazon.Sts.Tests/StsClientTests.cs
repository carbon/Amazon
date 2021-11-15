namespace Amazon.Sts.Tests;

public class StsClientTests
{
    [Fact]
    public void VersionIsCorrect()
    {
        Assert.Equal("2011-06-15", StsClient.Version);
    }
}
