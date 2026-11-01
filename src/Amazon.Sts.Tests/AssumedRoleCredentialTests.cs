using System.Net;
using System.Reflection;

using Amazon.Security;
using Amazon.Sts.Exceptions;

namespace Amazon.Sts.Tests;

public class AssumedRoleCredentialTests
{
    private const string RoleArn = "arn:aws:iam::123456789012:role/demo";

    [Fact]
    public void IsEmptyAndDueForRenewalBeforeFirstRenew()
    {
        var credential = new AssumedRoleCredential(FakeSts.Client(new FakeStsHandler()), RoleArn, "session");

        Assert.True(credential.ShouldRenew);
        Assert.Null(credential.AccessKeyId);
        Assert.Null(credential.SecretAccessKey);
        Assert.Null(credential.SecurityToken);
    }

    [Fact]
    public async Task RenewAsync_AssumesRoleAndExposesCredentials()
    {
        var handler = new FakeStsHandler(FakeSts.Xml(FakeSts.AssumeRoleXml(DateTime.UtcNow.AddMinutes(15))));

        var credential = new AssumedRoleCredential(FakeSts.Client(handler), RoleArn, "session");

        Assert.True(await credential.RenewAsync());

        Assert.False(credential.ShouldRenew);
        Assert.Equal("FAKETEMPKEYID",    credential.AccessKeyId);
        Assert.Equal("fake-temp-secret", credential.SecretAccessKey);
        Assert.Equal("fake-temp-token",  credential.SecurityToken);

        var form = Assert.Single(handler.Requests).Form;

        Assert.Equal("AssumeRole", form["Action"]);
        Assert.Equal(RoleArn,      form["RoleArn"]);
        Assert.Equal("session",    form["RoleSessionName"]);
        Assert.Equal("900",        form["DurationSeconds"]);
    }

    [Fact]
    public async Task RenewAsync_OmitsExternalIdByDefault()
    {
        var handler = new FakeStsHandler(FakeSts.Xml(FakeSts.AssumeRoleXml(DateTime.UtcNow.AddMinutes(15))));

        await new AssumedRoleCredential(FakeSts.Client(handler), RoleArn, "session").RenewAsync();

        Assert.DoesNotContain("ExternalId", Assert.Single(handler.Requests).Form.Keys);
    }

    [Fact]
    public async Task RenewAsync_SendsExternalIdOnEveryRenewal()
    {
        var handler = new FakeStsHandler(FakeSts.Xml(FakeSts.AssumeRoleXml(DateTime.UtcNow.AddMinutes(15))));

        var credential = new AssumedRoleCredential(FakeSts.Client(handler), RoleArn, "session", externalId: "fake-external-id");

        await credential.RenewAsync();
        await credential.RenewAsync();

        Assert.Equal(2, handler.Requests.Count);
        Assert.All(handler.Requests, static request => Assert.Equal("fake-external-id", request.Form["ExternalId"]));
    }

    [Fact]
    public async Task RegionConstructor_SendsExternalId()
    {
        var handler = new FakeStsHandler(FakeSts.Xml(FakeSts.AssumeRoleXml(DateTime.UtcNow.AddMinutes(15))));

        var credential = new AssumedRoleCredential(AwsRegion.EUWest1, FakeSts.Credential, RoleArn, "session", externalId: "fake-external-id");

        var client = (StsClient)typeof(AssumedRoleCredential)
            .GetField("_stsClient", BindingFlags.NonPublic | BindingFlags.Instance)!
            .GetValue(credential)!;

        FakeSts.Use(client, handler);

        await credential.RenewAsync();

        Assert.Equal("fake-external-id", Assert.Single(handler.Requests).Form["ExternalId"]);
    }

    [Theory]
    [InlineData(-1, true)]
    [InlineData(4, true)]
    [InlineData(6, false)]
    public async Task ShouldRenew_WithinFiveMinutesOfExpiration(int minutesLeft, bool expected)
    {
        var handler = new FakeStsHandler(FakeSts.Xml(FakeSts.AssumeRoleXml(DateTime.UtcNow.AddMinutes(minutesLeft))));

        var credential = new AssumedRoleCredential(FakeSts.Client(handler), RoleArn, "session");

        await credential.RenewAsync();

        Assert.Equal(expected, credential.ShouldRenew);
    }

    [Fact]
    public async Task RenewAsync_ReplacesCredentials()
    {
        var handler = new FakeStsHandler(
            FakeSts.Xml(FakeSts.AssumeRoleXml(DateTime.UtcNow.AddMinutes(1))),
            FakeSts.Xml(FakeSts.AssumeRoleXml(DateTime.UtcNow.AddMinutes(15), accessKeyId: "FAKESECONDKEYID", secret: "fake-second-secret", token: "fake-second-token")));

        var credential = new AssumedRoleCredential(FakeSts.Client(handler), RoleArn, "session");

        await credential.RenewAsync();
        Assert.True(credential.ShouldRenew);

        await credential.RenewAsync();
        Assert.False(credential.ShouldRenew);

        Assert.Equal("FAKESECONDKEYID",    credential.AccessKeyId);
        Assert.Equal("fake-second-secret", credential.SecretAccessKey);
        Assert.Equal("fake-second-token",  credential.SecurityToken);
    }

    [Fact]
    public async Task RenewAsync_Failure_KeepsCurrentCredentials()
    {
        var handler = new FakeStsHandler(
            FakeSts.Xml(FakeSts.AssumeRoleXml(DateTime.UtcNow.AddMinutes(1))),
            FakeSts.Xml(FakeSts.AccessDeniedXml, HttpStatusCode.Forbidden));

        var credential = new AssumedRoleCredential(FakeSts.Client(handler), RoleArn, "session");

        await credential.RenewAsync();

        await Assert.ThrowsAsync<StsException>(credential.RenewAsync);

        Assert.Equal("FAKETEMPKEYID", credential.AccessKeyId);
        Assert.True(credential.ShouldRenew);
    }

    [Fact]
    public async Task RegionConstructor_SignsWithSourceCredentialInRegion()
    {
        var handler = new FakeStsHandler(FakeSts.Xml(FakeSts.AssumeRoleXml(DateTime.UtcNow.AddMinutes(15))));

        var credential = new AssumedRoleCredential(AwsRegion.EUWest1, FakeSts.Credential, RoleArn, "session");

        var client = (StsClient)typeof(AssumedRoleCredential)
            .GetField("_stsClient", BindingFlags.NonPublic | BindingFlags.Instance)!
            .GetValue(credential)!;

        FakeSts.Use(client, handler);

        await credential.RenewAsync();

        var request = Assert.Single(handler.Requests);

        Assert.Equal("sts.eu-west-1.amazonaws.com", request.RequestUri.Host);
        Assert.Equal(FakeSts.AccessKeyId, new ParsedAuthorization(request.Headers["Authorization"]).AccessKeyId);
    }

    [Theory]
    [InlineData(null, "session")]
    [InlineData("", "session")]
    [InlineData(RoleArn, null)]
    [InlineData(RoleArn, "")]
    public void Constructors_RequireRoleAndSession(string? roleArn, string? session)
    {
        Assert.ThrowsAny<ArgumentException>(() => new AssumedRoleCredential(AwsRegion.USEast1, FakeSts.Credential, roleArn!, session!));
        Assert.ThrowsAny<ArgumentException>(() => new AssumedRoleCredential(FakeSts.Client(new FakeStsHandler()), roleArn!, session!));
    }

    [Fact]
    public async Task UsedByClient_RenewsOnceAndSignsWithSessionToken()
    {
        var stsHandler = new FakeStsHandler(FakeSts.Xml(FakeSts.AssumeRoleXml(DateTime.UtcNow.AddMinutes(15))));
        var credential = new AssumedRoleCredential(FakeSts.Client(stsHandler), RoleArn, "session");

        var handler = new FakeStsHandler(FakeSts.Xml(FakeSts.CallerIdentityXml));
        var client = FakeSts.Client(handler, credential: credential);

        await client.GetCallerIdentityAsync();
        await client.GetCallerIdentityAsync();

        Assert.Single(stsHandler.Requests);
        Assert.Equal(2, handler.Requests.Count);

        var request = handler.Requests[1];

        Assert.Equal("fake-temp-token", request.Headers["x-amz-security-token"]);

        var authorization = new ParsedAuthorization(request.Headers["Authorization"]);

        Assert.Equal("FAKETEMPKEYID", authorization.AccessKeyId);
        Assert.Contains("x-amz-security-token", authorization.SignedHeaders);
    }
}
