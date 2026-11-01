using System.Net;
using System.Text.Json;

using Amazon.Security;
using Amazon.Sts.Exceptions;

namespace Amazon.Sts.Tests;

public class CallerIdentityVerifierTests
{
    private const string ValidUrl = "https://sts.us-east-1.amazonaws.com/";

    private static CallerIdentityVerificationParameters Parameters(string url)
    {
        return new CallerIdentityVerificationParameters(
            url     : url,
            headers : new Dictionary<string, string> { { "x-amz-date", "20300102T030405Z" }, { "Authorization", "fake-authorization" } },
            body    : "Action=GetCallerIdentity&Version=2011-06-15"
        );
    }

    [Fact]
    public async Task ForwardsParametersAndReturnsIdentity()
    {
        var handler = new FakeStsHandler(FakeSts.Xml(FakeSts.CallerIdentityXml));

        var result = await FakeSts.Verifier(handler).VerifyCallerIdentityAsync(Parameters(ValidUrl));

        Assert.Equal("arn:aws:iam::123456789012:user/fake", result.Arn);
        Assert.Equal("FAKEUSERID",                          result.UserId);
        Assert.Equal("123456789012",                        result.Account);

        var request = Assert.Single(handler.Requests);

        Assert.Equal(HttpMethod.Post, request.Method);
        Assert.Equal(ValidUrl, request.RequestUri.ToString());
        Assert.Equal("Action=GetCallerIdentity&Version=2011-06-15", request.Body);
        Assert.StartsWith("application/x-www-form-urlencoded", request.ContentType);
        Assert.Equal("sts.us-east-1.amazonaws.com", request.Headers["Host"]);
        Assert.Equal("20300102T030405Z", request.Headers["x-amz-date"]);
        Assert.Equal("fake-authorization", request.Headers["Authorization"]);
    }

    // https://docs.aws.amazon.com/general/latest/gr/sts.html
    [Theory]
    [InlineData("https://sts.amazonaws.com/")]
    [InlineData("https://sts.us-east-1.amazonaws.com/")]
    [InlineData("https://sts.ap-southeast-3.amazonaws.com/")]
    [InlineData("https://sts.us-gov-east-1.amazonaws.com/")]
    [InlineData("https://STS.US-EAST-1.AMAZONAWS.COM/")]
    public async Task AcceptsStsEndpoints(string url)
    {
        var handler = new FakeStsHandler(FakeSts.Xml(FakeSts.CallerIdentityXml));

        await FakeSts.Verifier(handler).VerifyCallerIdentityAsync(Parameters(url));

        Assert.Single(handler.Requests);
    }

    [Fact]
    public async Task RejectsHttp()
    {
        var handler = new FakeStsHandler(FakeSts.Xml(FakeSts.CallerIdentityXml));

        await Assert.ThrowsAsync<ArgumentException>(() => FakeSts.Verifier(handler).VerifyCallerIdentityAsync(Parameters("http://sts.us-east-1.amazonaws.com/")));

        Assert.Empty(handler.Requests);
    }

    [Theory]
    [InlineData("https://example.com/")]
    [InlineData("https://sts.example.com/")]
    [InlineData("https://sts.amazonaws.com.example.com/")]
    [InlineData("https://sts.us-east-1.amazonaws.com@example.com/")]
    [InlineData("https://example.com/sts.us-east-1.amazonaws.com")]
    [InlineData("https://example.com/?host=sts.amazonaws.com")]
    [InlineData("https://notsts.amazonaws.com/")]
    [InlineData("https://iam.amazonaws.com/")]
    [InlineData("https://sts.s3.amazonaws.com/")]
    [InlineData("https://sts.s3-us-west-2.amazonaws.com/")]
    [InlineData("https://sts.example.amazonaws.com/")]
    [InlineData("https://sts.example.s3.amazonaws.com/")]
    [InlineData("https://sts.example.s3.us-east-1.amazonaws.com/")]
    [InlineData("https://sts.example.execute-api.us-east-1.amazonaws.com/")]
    [InlineData("https://sts.us-east-1.example.amazonaws.com/")]
    public async Task RejectsOtherHosts_WithoutSendingTheSignedRequest(string url)
    {
        var handler = new FakeStsHandler(FakeSts.Xml(FakeSts.CallerIdentityXml));

        var ex = await Assert.ThrowsAsync<Exception>(() => FakeSts.Verifier(handler).VerifyCallerIdentityAsync(Parameters(url)));

        Assert.Contains("Must be an STS endpoint", ex.Message);
        Assert.Empty(handler.Requests);
    }

    [Fact]
    public async Task ErrorResponse_ThrowsStsException()
    {
        var handler = new FakeStsHandler(FakeSts.Xml(FakeSts.AccessDeniedXml, HttpStatusCode.Forbidden));

        var ex = await Assert.ThrowsAsync<StsException>(() => FakeSts.Verifier(handler).VerifyCallerIdentityAsync(Parameters(ValidUrl)));

        Assert.Equal(HttpStatusCode.Forbidden, ex.HttpStatusCode);
        Assert.Contains("<Code>AccessDenied</Code>", ex.Message);
    }

    [Theory]
    [InlineData("not xml")]
    [InlineData("<GetSessionTokenResponse xmlns=\"https://sts.amazonaws.com/doc/2011-06-15/\" />")]
    public async Task UnexpectedSuccessBody_DoesNotVerify(string body)
    {
        var handler = new FakeStsHandler(FakeSts.Xml(body));

        await Assert.ThrowsAsync<InvalidOperationException>(() => FakeSts.Verifier(handler).VerifyCallerIdentityAsync(Parameters(ValidUrl)));
    }

    [Fact]
    public async Task RequestSentByVerifierHasValidSignature()
    {
        var parameters = await new StsClient(AwsRegion.USWest2, FakeSts.Credential).GetCallerIdentityVerificationParametersAsync();

        // The parameters are handed to the verifying party as JSON
        parameters = JsonSerializer.Deserialize<CallerIdentityVerificationParameters>(JsonSerializer.Serialize(parameters))!;

        var handler = new FakeStsHandler(FakeSts.Xml(FakeSts.CallerIdentityXml));

        await FakeSts.Verifier(handler).VerifyCallerIdentityAsync(parameters);

        var request = Assert.Single(handler.Requests);

        var authorization = new ParsedAuthorization(request.Headers["Authorization"]);

        Assert.Equal(FakeSts.AccessKeyId, authorization.AccessKeyId);

        // The verifier adds a Content-Type, which must not have been signed
        Assert.Equal("host;x-amz-date", authorization.SignedHeaders);

        Assert.Equal(authorization.Signature, FakeSts.ComputeSignature(FakeSts.SecretAccessKey, authorization.Scope, authorization.SignedHeaders, request.Headers, request.Body));
    }

    [Fact]
    public async Task RequestSentByVerifierHasValidSignature_WithSessionToken()
    {
        var stsHandler = new FakeStsHandler(FakeSts.Xml(FakeSts.AssumeRoleXml(DateTime.UtcNow.AddMinutes(15))));
        var credential = new AssumedRoleCredential(FakeSts.Client(stsHandler), "arn:aws:iam::123456789012:role/demo", "session");

        var parameters = await new StsClient(AwsRegion.USEast1, credential).GetCallerIdentityVerificationParametersAsync();

        Assert.Equal("fake-temp-token", parameters.Headers["x-amz-security-token"]);
        Assert.Equal(3, parameters.Headers.Count);

        var handler = new FakeStsHandler(FakeSts.Xml(FakeSts.CallerIdentityXml));

        await FakeSts.Verifier(handler).VerifyCallerIdentityAsync(parameters);

        var request = Assert.Single(handler.Requests);

        var authorization = new ParsedAuthorization(request.Headers["Authorization"]);

        Assert.Equal("FAKETEMPKEYID", authorization.AccessKeyId);
        Assert.Equal("host;x-amz-date;x-amz-security-token", authorization.SignedHeaders);

        Assert.Equal(authorization.Signature, FakeSts.ComputeSignature("fake-temp-secret", authorization.Scope, authorization.SignedHeaders, request.Headers, request.Body));
    }
}
