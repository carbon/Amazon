using System.Text.Json;

using Amazon.Sts.Tests;

namespace Amazon.Sts.Requests.Tests;

public class RequestParameterTests
{
    [Theory]
    [InlineData(15 * 60, 900)]
    [InlineData(12 * 60 * 60, 43200)]
    public void AssumeRole_AcceptsDurationLimits(int seconds, int expected)
    {
        var request = new AssumeRoleRequest("arn", "session", TimeSpan.FromSeconds(seconds));

        Assert.Equal(expected, request.DurationSeconds);
    }

    [Theory]
    [InlineData(15 * 60 - 1)]
    [InlineData(12 * 60 * 60 + 1)]
    public void AssumeRole_RejectsDurationOutsideLimits(int seconds)
    {
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() => new AssumeRoleRequest("arn", "session", TimeSpan.FromSeconds(seconds)));

        Assert.Equal("duration", ex.ParamName);
    }

    [Theory]
    [InlineData(null, "session")]
    [InlineData("", "session")]
    [InlineData("arn", null)]
    [InlineData("arn", "")]
    public void AssumeRole_RequiresRoleAndSession(string? roleArn, string? session)
    {
        Assert.ThrowsAny<ArgumentException>(() => new AssumeRoleRequest(roleArn!, session!));
    }

    [Fact]
    public void AssumeRole_OmitsUnsetDurationAndPolicy()
    {
        var parameters = StsRequestHelper.ToParams(new AssumeRoleRequest("arn", "session"));

        Assert.Equal(["Action", "RoleArn", "RoleSessionName"], parameters.Select(static p => p.Key));
    }

    [Fact]
    public void AssumeRole_SendsPolicyAsJsonDocument()
    {
        const string policy = """{"Version":"2012-10-17","Statement":[{"Effect":"Allow","Action":"s3:*","Resource":"*"}]}""";

        using var doc = JsonDocument.Parse(policy);

        var parameters = StsRequestHelper.ToParams(new AssumeRoleRequest("arn", "session", policy: doc.RootElement));

        Assert.Equal(new("Policy", policy), Assert.Single(parameters, static p => p.Key is "Policy"));
    }

    [Fact]
    public void ToParams_DoesNotJsonEscapeValues()
    {
        var parameters = StsRequestHelper.ToParams(new AssumeRoleRequest("arn", "a+b=<c>&'d'"));

        Assert.Equal(new("RoleSessionName", "a+b=<c>&'d'"), parameters[2]);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void GetFederationToken_RequiresName(string? name)
    {
        Assert.ThrowsAny<ArgumentException>(() => new GetFederationTokenRequest(name!));
    }

    [Fact]
    public void GetFederationToken_OmitsUnsetParameters()
    {
        var parameters = StsRequestHelper.ToParams(new GetFederationTokenRequest("Bob"));

        Assert.Equal(["Action", "Name"], parameters.Select(static p => p.Key));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void DecodeAuthorizationMessage_RequiresMessage(string? message)
    {
        Assert.ThrowsAny<ArgumentException>(() => new DecodeAuthorizationMessageRequest(message!));
    }
}
