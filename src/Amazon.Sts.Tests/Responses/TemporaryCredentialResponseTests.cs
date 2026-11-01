using Amazon.Sts.Serialization;
using Amazon.Sts.Tests;

namespace Amazon.Sts.Responses.Tests;

// The samples follow the shapes in https://docs.aws.amazon.com/STS/latest/APIReference/, with fake values
internal static class ResponseSamples
{
    public const string GetSessionToken =
        """
        <GetSessionTokenResponse xmlns="https://sts.amazonaws.com/doc/2011-06-15/">
          <GetSessionTokenResult>
            <Credentials>
              <SessionToken>fake-temp-token</SessionToken>
              <SecretAccessKey>fake-temp-secret</SecretAccessKey>
              <Expiration>2011-07-11T19:55:29.611Z</Expiration>
              <AccessKeyId>FAKETEMPKEYID</AccessKeyId>
            </Credentials>
          </GetSessionTokenResult>
          <ResponseMetadata>
            <RequestId>58c5dbae-abef-11e0-8cfe-09039844ac7d</RequestId>
          </ResponseMetadata>
        </GetSessionTokenResponse>
        """;

    public const string GetFederationToken =
        """
        <GetFederationTokenResponse xmlns="https://sts.amazonaws.com/doc/2011-06-15/">
          <GetFederationTokenResult>
            <Credentials>
              <SessionToken>fake-temp-token</SessionToken>
              <SecretAccessKey>fake-temp-secret</SecretAccessKey>
              <Expiration>2011-07-15T23:28:33.359Z</Expiration>
              <AccessKeyId>FAKETEMPKEYID</AccessKeyId>
            </Credentials>
            <FederatedUser>
              <Arn>arn:aws:sts::123456789012:federated-user/Bob</Arn>
              <FederatedUserId>123456789012:Bob</FederatedUserId>
            </FederatedUser>
            <PackedPolicySize>6</PackedPolicySize>
          </GetFederationTokenResult>
          <ResponseMetadata>
            <RequestId>c6104cbe-af31-11e0-8154-cbc7ccf896c7</RequestId>
          </ResponseMetadata>
        </GetFederationTokenResponse>
        """;

    public const string AssumeRoleWithSAML =
        """
        <AssumeRoleWithSAMLResponse xmlns="https://sts.amazonaws.com/doc/2011-06-15/">
          <AssumeRoleWithSAMLResult>
            <Issuer>https://integ.example.com/idp/shibboleth</Issuer>
            <AssumedRoleUser>
              <Arn>arn:aws:sts::123456789012:assumed-role/TestSaml</Arn>
              <AssumedRoleId>FAKEROLEID:TestSaml</AssumedRoleId>
            </AssumedRoleUser>
            <Credentials>
              <AccessKeyId>FAKETEMPKEYID</AccessKeyId>
              <SecretAccessKey>fake-temp-secret</SecretAccessKey>
              <SessionToken>fake-temp-token</SessionToken>
              <Expiration>2019-11-01T20:26:47Z</Expiration>
            </Credentials>
            <Audience>https://signin.aws.amazon.com/saml</Audience>
            <SubjectType>transient</SubjectType>
            <PackedPolicySize>6</PackedPolicySize>
            <NameQualifier>fake-name-qualifier</NameQualifier>
            <SourceIdentity>SourceIdentityValue</SourceIdentity>
            <Subject>SamlExample</Subject>
          </AssumeRoleWithSAMLResult>
          <ResponseMetadata>
            <RequestId>c6104cbe-af31-11e0-8154-cbc7ccf896c7</RequestId>
          </ResponseMetadata>
        </AssumeRoleWithSAMLResponse>
        """;

    // The wire shape comes from the AWS SDK service model (botocore), which has resultWrapper: DecodeAuthorizationMessageResult.
    // The sample in the API reference omits the wrapper and shows an http:// namespace.
    public const string DecodeAuthorizationMessage =
        """
        <DecodeAuthorizationMessageResponse xmlns="https://sts.amazonaws.com/doc/2011-06-15/">
          <DecodeAuthorizationMessageResult>
            <DecodedMessage>{"allowed":"false"}</DecodedMessage>
          </DecodeAuthorizationMessageResult>
          <ResponseMetadata>
            <RequestId>6624a9ca-cd25-4f50-b2a5-7ba65bf07453</RequestId>
          </ResponseMetadata>
        </DecodeAuthorizationMessageResponse>
        """;
}

public class TemporaryCredentialResponseTests
{
    [Fact]
    public void GetSessionToken_ParsesFractionalExpirationAsUtc()
    {
        var credentials = StsXmlSerializer<GetSessionTokenResponse>.Deserialize(Utf8(ResponseSamples.GetSessionToken)).GetSessionTokenResult.Credentials;

        Assert.Equal("FAKETEMPKEYID",    credentials.AccessKeyId);
        Assert.Equal("fake-temp-secret", credentials.SecretAccessKey);
        Assert.Equal("fake-temp-token",  credentials.SessionToken);

        // ExpiresIn subtracts UtcNow, so a local time here would be off by the host's UTC offset
        Assert.Equal(DateTimeKind.Utc, credentials.Expiration.Kind);
        Assert.Equal(new DateTime(2011, 07, 11, 19, 55, 29, 611, DateTimeKind.Utc), credentials.Expiration);
    }

    [Fact]
    public void GetFederationToken_ParsesExpirationAsUtc()
    {
        var credentials = StsXmlSerializer<GetFederationTokenResponse>.Deserialize(Utf8(ResponseSamples.GetFederationToken)).GetFederationTokenResult.Credentials;

        Assert.Equal(DateTimeKind.Utc, credentials.Expiration.Kind);
        Assert.Equal(new DateTime(2011, 07, 15, 23, 28, 33, 359, DateTimeKind.Utc), credentials.Expiration);
    }

    [Fact]
    public void AssumeRole_OptionalElementsMayBeAbsent()
    {
        var result = StsXmlSerializer<AssumeRoleResponse>.Deserialize(Utf8(FakeSts.AssumeRoleXml(DateTime.UtcNow))).AssumeRoleResult;

        Assert.Null(result.SourceIdentity);
        Assert.Equal(0, result.PackedPolicySize);
    }

    [Fact]
    public void ExpiresIn_IsRelativeToNow()
    {
        var remaining = Credentials(DateTime.UtcNow.AddHours(1)).ExpiresIn;

        Assert.InRange(remaining, TimeSpan.FromMinutes(59), TimeSpan.FromHours(1));

        Assert.True(Credentials(DateTime.UtcNow.AddHours(-1)).ExpiresIn < TimeSpan.Zero);
    }

    [Fact]
    public void AssumeRoleWithSAML_CanDeserializeSample()
    {
        // https://docs.aws.amazon.com/STS/latest/APIReference/API_AssumeRoleWithSAML.html
        var result = StsXmlSerializer<AssumeRoleWithSamlResponse>.Deserialize(Utf8(ResponseSamples.AssumeRoleWithSAML)).AssumeRoleWithSamlResult;

        Assert.Equal("https://integ.example.com/idp/shibboleth",        result.Issuer);
        Assert.Equal("https://signin.aws.amazon.com/saml",              result.Audience);
        Assert.Equal("transient",                                       result.SubjectType);
        Assert.Equal("SamlExample",                                     result.Subject);
        Assert.Equal("fake-name-qualifier",                             result.NameQualifier);
        Assert.Equal("SourceIdentityValue",                             result.SourceIdentity);
        Assert.Equal(6,                                                 result.PackedPolicySize);
        Assert.Equal("arn:aws:sts::123456789012:assumed-role/TestSaml", result.AssumedRoleUser.Arn);
        Assert.Equal("FAKEROLEID:TestSaml",                             result.AssumedRoleUser.AssumedRoleId);
        Assert.Equal("FAKETEMPKEYID",                                   result.Credentials.AccessKeyId);
        Assert.Equal(new DateTime(2019, 11, 01, 20, 26, 47, DateTimeKind.Utc), result.Credentials.Expiration);
        Assert.Equal(DateTimeKind.Utc,                                  result.Credentials.Expiration.Kind);
    }

    [Fact]
    public void DecodeAuthorizationMessage_ReadsDecodedMessage()
    {
        var response = StsXmlSerializer<DecodeAuthorizationMessageResponse>.Deserialize(Utf8(ResponseSamples.DecodeAuthorizationMessage));

        Assert.Equal("""{"allowed":"false"}""", response.DecodeAuthorizationMessageResult.DecodedMessage);
    }

    private static Credentials Credentials(DateTime expiration)
    {
        return new Credentials {
            AccessKeyId     = "FAKETEMPKEYID",
            SecretAccessKey = "fake-temp-secret",
            SessionToken    = "fake-temp-token",
            Expiration      = expiration
        };
    }

    private static byte[] Utf8(string text) => System.Text.Encoding.UTF8.GetBytes(text);
}
