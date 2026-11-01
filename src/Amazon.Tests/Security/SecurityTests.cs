using System.Globalization;
using System.Net;
using System.Security.Cryptography;
using System.Text;

using Amazon.Exceptions;

namespace Amazon.Security.Tests;

public class AwsSessionTests
{
    private static AwsSession Create() => new(
        sessionToken    : "token",
        secretAccessKey : "secret",
        expiration      : new DateTime(2020, 01, 01),
        accessKeyId     : "key");

    [Fact]
    public async Task SetsProperties()
    {
        var session = Create();

        Assert.Equal("token", session.SessionToken);
        Assert.Equal("secret", session.SecretAccessKey);
        Assert.Equal("key", session.AccessKeyId);
        Assert.Equal(new DateTime(2020, 01, 01), session.Expiration);
        Assert.False(session.ShouldRenew);
        await Assert.ThrowsAsync<NotImplementedException>(() => session.RenewAsync());
    }

    [Fact]
    public void SecurityTokenDefaultsToSessionToken()
    {
        Assert.Equal("token", Create().SecurityToken);
    }

    [Fact]
    public void ExplicitSecurityTokenWins()
    {
        Assert.Equal("other", new AwsSession("token", "secret", default, "key", securityToken: "other").SecurityToken);
    }
}

public class AwsExceptionTests
{
    [Fact]
    public void KeepsStatusCodeAndInnerException()
    {
        var inner = new Exception("inner");

        var ex = new AwsException("message", inner, HttpStatusCode.BadGateway);

        Assert.Equal("message", ex.Message);
        Assert.Same(inner, ex.InnerException);
        Assert.Equal(HttpStatusCode.BadGateway, ex.HttpStatusCode);
        Assert.Equal(HttpStatusCode.NotFound, new AwsException("message", HttpStatusCode.NotFound).HttpStatusCode);
    }

    [Fact]
    public void ServiceUnavailableIsTransient()
    {
        var ex = new ServiceUnavailableException();

        Assert.Equal("Service unavailable", ex.Message);
        Assert.Equal(HttpStatusCode.ServiceUnavailable, ex.HttpStatusCode);
        Assert.True(ex.IsTransient);
        Assert.Equal("Down", new ServiceUnavailableException("Down").Message);
    }
}

public class CredentialScopeFormattingTests
{
    private static readonly CredentialScope s_scope = new(new DateOnly(2012, 02, 15), AwsRegion.USEast1, AwsService.Iam);

    private const string Expected = "20120215/us-east-1/iam/aws4_request";

    [Fact]
    public void ValidatesArguments()
    {
        Assert.Throws<ArgumentNullException>(() => new CredentialScope(default, null, AwsService.Iam));
        Assert.Throws<ArgumentNullException>(() => new CredentialScope(default, AwsRegion.USEast1, null));
    }

    [Fact]
    public void SupportsFormatting()
    {
        Assert.Equal(Expected, string.Format(CultureInfo.InvariantCulture, "{0}", s_scope));
        Assert.Equal(Expected, ((IFormattable)s_scope).ToString(null, CultureInfo.InvariantCulture));
    }

    [Fact]
    public void TryFormat_WritesWhenItFits()
    {
        Span<char> buffer = stackalloc char[64];

        Assert.True(((ISpanFormattable)s_scope).TryFormat(buffer, out int written, default, CultureInfo.InvariantCulture));
        Assert.Equal(Expected, buffer[..written].ToString());
    }

    [Fact]
    public void TryFormat_FailsWhenTooSmall()
    {
        Span<char> buffer = stackalloc char[4];

        Assert.False(((ISpanFormattable)s_scope).TryFormat(buffer, out _, default, CultureInfo.InvariantCulture));
    }
}

public class SignerV4AdditionalTests
{
    private static readonly AwsCredential s_credential = new("key", "wJalrXUtnFEMI/K7MDENG+bPxRfiCYEXAMPLEKEY");

    private static readonly CredentialScope s_s3Scope = new(new DateOnly(2012, 02, 15), AwsRegion.USEast1, AwsService.S3);

    private static HttpRequestMessage CreateRequest(HttpContent content = null)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, "https://s3.amazonaws.com/bucket/key") { Content = content };

        request.Headers.Date = new DateTimeOffset(2012, 02, 17, 18, 31, 22, TimeSpan.Zero);
        request.Headers.Host = "s3.amazonaws.com";
        request.Headers.Add("x-amz-date", "20120217T183122Z");

        return request;
    }

    [Fact]
    public void RejectsVeryLongSecrets()
    {
        var ex = Assert.Throws<ArgumentException>(() => SignerV4.ComputeSigningKey(new string('a', 257), s_s3Scope));

        Assert.Equal("secretAccessKey", ex.ParamName);
    }

    [Fact]
    public void Sign_HashesS3ContentWhenHeaderIsMissing()
    {
        var request = CreateRequest(new StringContent("hello"));

        SignerV4.Sign(s_credential, s_s3Scope, request);

        string expected = Convert.ToHexStringLower(SHA256.HashData(Encoding.UTF8.GetBytes("hello")));

        Assert.Equal(expected, request.Headers.GetValues("x-amz-content-sha256").Single());
        Assert.Contains("x-amz-content-sha256", request.Headers.NonValidated["Authorization"].ToString());
    }

    [Fact]
    public void Sign_HashesEmptyS3Content()
    {
        var request = CreateRequest();

        SignerV4.Sign(s_credential, s_s3Scope, request);

        Assert.Equal("e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855", request.Headers.GetValues("x-amz-content-sha256").Single());
    }

    [Fact]
    public void Sign_KeepsExistingS3ContentHash()
    {
        var request = CreateRequest(new StringContent("hello"));

        request.Headers.TryAddWithoutValidation("x-amz-content-sha256", "UNSIGNED-PAYLOAD");

        SignerV4.Sign(s_credential, s_s3Scope, request);

        Assert.Equal("UNSIGNED-PAYLOAD", request.Headers.GetValues("x-amz-content-sha256").Single());
    }

    [Fact]
    public void Presign_IncludesSecurityToken()
    {
        var credential = new AwsSession("token", "secret", default, "key", securityToken: "session token");

        var url = SignerV4.GetPresignedUrl(
            credential : credential,
            scope      : s_s3Scope,
            date       : new DateTime(2012, 02, 17, 18, 31, 22, DateTimeKind.Utc),
            expires    : TimeSpan.FromMinutes(1),
            method     : HttpMethod.Get,
            requestUri : new Uri("https://s3.amazonaws.com/bucket/key"));

        Assert.Contains("X-Amz-Security-Token=session%20token", url);
        Assert.Contains("X-Amz-Expires=60", url);
    }

    [Fact]
    public void Presign_RewritesRequestUri()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "https://s3.amazonaws.com/bucket/key");

        SignerV4.Presign(
            credential : s_credential,
            scope      : s_s3Scope,
            date       : new DateTime(2012, 02, 17, 18, 31, 22, DateTimeKind.Utc),
            expires    : TimeSpan.FromMinutes(1),
            request    : request);

        Assert.Contains("X-Amz-Signature=", request.RequestUri!.Query);
        Assert.Throws<ArgumentNullException>(() => SignerV4.Presign(s_credential, s_s3Scope, default, default, null));
    }

    [Theory]
    [InlineData("https://s3.amazonaws.com/")]
    [InlineData("https://s3.amazonaws.com/?")]
    public void CanonicalizeQueryString_EmptyWhenNoQuery(string url)
    {
        Assert.Equal("", SignerV4.CanonicalizeQueryString(new Uri(url)));
    }
}
