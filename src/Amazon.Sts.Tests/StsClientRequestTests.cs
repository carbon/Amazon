using System.Net;

using Amazon.Sts.Exceptions;
using Amazon.Sts.Responses.Tests;

namespace Amazon.Sts.Tests;

public class StsClientRequestTests
{
    private static readonly DateTime s_expiration = new(2030, 01, 02, 03, 04, 05, DateTimeKind.Utc);

    [Fact]
    public async Task AssumeRole_PostsSignedFormToRegionalEndpoint()
    {
        var handler = new FakeStsHandler(FakeSts.Xml(FakeSts.AssumeRoleXml(s_expiration)));
        var client = FakeSts.Client(handler, AwsRegion.EUWest1);

        var response = await client.AssumeRoleAsync(new AssumeRoleRequest("arn:aws:iam::123456789012:role/demo", "session name", TimeSpan.FromHours(1)));

        Assert.Equal("FAKETEMPKEYID", response.AssumeRoleResult.Credentials.AccessKeyId);

        var request = Assert.Single(handler.Requests);

        Assert.Equal(HttpMethod.Post, request.Method);
        Assert.Equal("https://sts.eu-west-1.amazonaws.com/", request.RequestUri.ToString());
        Assert.Equal("application/x-www-form-urlencoded", request.ContentType);

        Assert.Equal("Action=AssumeRole&DurationSeconds=3600&RoleArn=arn%3Aaws%3Aiam%3A%3A123456789012%3Arole%2Fdemo&RoleSessionName=session+name&Version=2011-06-15", request.Body);

        var authorization = new ParsedAuthorization(request.Headers["Authorization"]);

        Assert.Equal(FakeSts.AccessKeyId, authorization.AccessKeyId);
        Assert.EndsWith("/eu-west-1/sts/aws4_request", authorization.Scope);
        Assert.Equal("content-type;host;x-amz-date", authorization.SignedHeaders);
    }

    [Fact]
    public async Task AssumeRoleWithWebIdentity_SendsParameters()
    {
        var handler = new FakeStsHandler(FakeSts.Xml(
            """
            <AssumeRoleWithWebIdentityResponse xmlns="https://sts.amazonaws.com/doc/2011-06-15/">
              <AssumeRoleWithWebIdentityResult>
                <SubjectFromWebIdentityToken>subject</SubjectFromWebIdentityToken>
                <Audience>audience</Audience>
                <AssumedRoleUser>
                  <Arn>arn:aws:sts::123456789012:assumed-role/demo/app</Arn>
                  <AssumedRoleId>FAKEROLEID:app</AssumedRoleId>
                </AssumedRoleUser>
                <Credentials>
                  <AccessKeyId>FAKETEMPKEYID</AccessKeyId>
                  <SecretAccessKey>fake-temp-secret</SecretAccessKey>
                  <SessionToken>fake-temp-token</SessionToken>
                  <Expiration>2030-01-02T03:04:05Z</Expiration>
                </Credentials>
                <Provider>www.example.com</Provider>
              </AssumeRoleWithWebIdentityResult>
            </AssumeRoleWithWebIdentityResponse>
            """));

        var response = await FakeSts.Client(handler).AssumeRoleWithWebIdentityAsync(new AssumeRoleWithWebIdentityRequest {
            RoleArn          = "arn:aws:iam::123456789012:role/demo",
            RoleSessionName  = "app",
            WebIdentityToken = "fake.web+identity/token",
            DurationSeconds  = 900,
            Policy           = """{"Version":"2012-10-17"}""",
            ProviderId       = "www.example.com"
        });

        Assert.Equal("www.example.com", response.AssumeRoleWithWebIdentityResult.Provider);

        var form = Assert.Single(handler.Requests).Form;

        Assert.Equal("AssumeRoleWithWebIdentity",           form["Action"]);
        Assert.Equal("900",                                 form["DurationSeconds"]);
        Assert.Equal("""{"Version":"2012-10-17"}""",        form["Policy"]);
        Assert.Equal("www.example.com",                     form["ProviderId"]);
        Assert.Equal("arn:aws:iam::123456789012:role/demo", form["RoleArn"]);
        Assert.Equal("app",                                 form["RoleSessionName"]);
        Assert.Equal("fake.web+identity/token",             form["WebIdentityToken"]);
        Assert.Equal("2011-06-15",                          form["Version"]);
    }

    [Fact]
    public async Task AssumeRoleWithSAML_SendsParameters()
    {
        var handler = new FakeStsHandler(FakeSts.Xml(ResponseSamples.AssumeRoleWithSAML));

        var response = await FakeSts.Client(handler).AssumeRoleWithSAMLAsync(new AssumeRoleWithSamlRequest {
            PrincipalArn    = "arn:aws:iam::123456789012:saml-provider/fake",
            RoleArn         = "arn:aws:iam::123456789012:role/demo",
            SAMLAssertion   = "ZmFrZS1hc3NlcnRpb24+Lw==",
            DurationSeconds = 900,
            Policy          = """{"Version":"2012-10-17"}"""
        });

        Assert.Equal("FAKETEMPKEYID", response.AssumeRoleWithSamlResult.Credentials.AccessKeyId);

        // https://docs.aws.amazon.com/STS/latest/APIReference/API_AssumeRoleWithSAML.html
        var form = Assert.Single(handler.Requests).Form;

        Assert.Equal("AssumeRoleWithSAML",                           form["Action"]);
        Assert.Equal("900",                                          form["DurationSeconds"]);
        Assert.Equal("""{"Version":"2012-10-17"}""",                 form["Policy"]);
        Assert.Equal("arn:aws:iam::123456789012:saml-provider/fake", form["PrincipalArn"]);
        Assert.Equal("arn:aws:iam::123456789012:role/demo",          form["RoleArn"]);
        Assert.Equal("ZmFrZS1hc3NlcnRpb24+Lw==",                     form["SAMLAssertion"]);
    }

    [Fact]
    public async Task DecodeAuthorizationMessage_SendsEncodedMessage()
    {
        var handler = new FakeStsHandler(FakeSts.Xml(ResponseSamples.DecodeAuthorizationMessage));

        var response = await FakeSts.Client(handler).DecodeAuthorizationMessageAsync(new DecodeAuthorizationMessageRequest("fake+encoded/message"));

        Assert.Equal("""{"allowed":"false"}""", response.DecodeAuthorizationMessageResult.DecodedMessage);

        var form = Assert.Single(handler.Requests).Form;

        Assert.Equal("DecodeAuthorizationMessage", form["Action"]);
        Assert.Equal("fake+encoded/message",       form["EncodedMessage"]);
    }

    [Fact]
    public async Task GetCallerIdentity_ReturnsIdentity()
    {
        var handler = new FakeStsHandler(FakeSts.Xml(FakeSts.CallerIdentityXml));

        var response = await FakeSts.Client(handler).GetCallerIdentityAsync();

        Assert.Equal("123456789012", response.GetCallerIdentityResult.Account);

        Assert.Equal("Action=GetCallerIdentity&Version=2011-06-15", Assert.Single(handler.Requests).Body);
    }

    [Fact]
    public async Task GetFederationToken_SendsParametersAndParsesResult()
    {
        var handler = new FakeStsHandler(FakeSts.Xml(ResponseSamples.GetFederationToken));

        var response = await FakeSts.Client(handler).GetFederationTokenAsync(new GetFederationTokenRequest("Bob", """{"Version":"2012-10-17"}""", 3600));

        var result = response.GetFederationTokenResult;

        Assert.Equal("arn:aws:sts::123456789012:federated-user/Bob", result.FederatedUser.Arn);
        Assert.Equal("123456789012:Bob",                             result.FederatedUser.FederatedUserId);
        Assert.Equal(6,                                              result.PackedPolicySize);
        Assert.Equal("FAKETEMPKEYID",                                result.Credentials.AccessKeyId);

        var form = Assert.Single(handler.Requests).Form;

        Assert.Equal("GetFederationToken",           form["Action"]);
        Assert.Equal("3600",                         form["DurationSeconds"]);
        Assert.Equal("Bob",                          form["Name"]);
        Assert.Equal("""{"Version":"2012-10-17"}""", form["Policy"]);
    }

    [Fact]
    public async Task GetSessionToken_OmitsUnsetParameters()
    {
        var handler = new FakeStsHandler(FakeSts.Xml(ResponseSamples.GetSessionToken));

        var response = await FakeSts.Client(handler).GetSessionTokenAsync(new GetSessionTokenRequest());

        Assert.Equal("FAKETEMPKEYID", response.GetSessionTokenResult.Credentials.AccessKeyId);

        Assert.Equal("Action=GetSessionToken&Version=2011-06-15", Assert.Single(handler.Requests).Body);
    }

    [Fact]
    public async Task GetSessionToken_SendsMfaParameters()
    {
        var handler = new FakeStsHandler(FakeSts.Xml(ResponseSamples.GetSessionToken));

        await FakeSts.Client(handler).GetSessionTokenAsync(new GetSessionTokenRequest {
            SerialNumber = "arn:aws:iam::123456789012:mfa/fake",
            TokenCode    = "123456"
        });

        var form = Assert.Single(handler.Requests).Form;

        Assert.Equal("arn:aws:iam::123456789012:mfa/fake", form["SerialNumber"]);
        Assert.Equal("123456",                             form["TokenCode"]);
    }

    [Fact]
    public void GetSessionToken_SendsDurationSeconds()
    {
        // https://docs.aws.amazon.com/STS/latest/APIReference/API_GetSessionToken.html
        var parameters = StsRequestHelper.ToParams(new GetSessionTokenRequest { DurationSeconds = 3600 });

        Assert.Contains(new KeyValuePair<string, string>("DurationSeconds", "3600"), parameters);
    }

    [Fact]
    public async Task ErrorResponse_ThrowsStsExceptionWithBodyAndStatus()
    {
        var handler = new FakeStsHandler(FakeSts.Xml(FakeSts.AccessDeniedXml, HttpStatusCode.Forbidden));

        var ex = await Assert.ThrowsAsync<StsException>(() => FakeSts.Client(handler).GetCallerIdentityAsync());

        Assert.Equal(HttpStatusCode.Forbidden, ex.HttpStatusCode);
        Assert.Contains("<Code>AccessDenied</Code>", ex.Message);

        // The client doesn't retry
        Assert.Single(handler.Requests);
    }

    [Theory]
    [InlineData("not xml")]
    [InlineData("<GetSomethingElseResponse xmlns=\"https://sts.amazonaws.com/doc/2011-06-15/\" />")]
    [InlineData("<GetCallerIdentityResponse xmlns=\"https://example.com/\" />")]
    public async Task UnexpectedSuccessBody_Throws(string body)
    {
        var handler = new FakeStsHandler(FakeSts.Xml(body));

        await Assert.ThrowsAsync<InvalidOperationException>(() => FakeSts.Client(handler).GetCallerIdentityAsync());
    }
}
