namespace Amazon.Sts.Tests;

public class GetCallerIdentityRequestTests
{
    private static readonly AwsCredential awsCredential = new ("public", "private");

    [Fact]
    public async Task A()
    {
        var client = new StsClient(AwsRegion.USEast1, awsCredential);

        var parameters = await client.GetCallerIdentityVerificationParametersAsync();

        Assert.Equal("Action=GetCallerIdentity&Version=2011-06-15", parameters.Body);

        Assert.Equal(2, parameters.Headers.Count);

        Assert.NotNull(parameters.Headers["x-amz-date"]);
        Assert.NotNull(parameters.Headers["Authorization"]);

        var age = parameters.GetAge();

        Assert.True(age > TimeSpan.Zero);
    }
}
