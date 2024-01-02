using Amazon.Sts.Serialization;

namespace Amazon.Sts.Responses.Tests;

public class AssumeRoleWithWebIdentityTests
{
    [Fact]
    public void CanDeserialize()
    {
        // sample via https://docs.aws.amazon.com/STS/latest/APIReference/API_AssumeRoleWithWebIdentity.html
        var response = StsXmlSerializer<AssumeRoleWithWebIdentityResponse>.Deserialize(
            """
            <AssumeRoleWithWebIdentityResponse xmlns="https://sts.amazonaws.com/doc/2011-06-15/">
              <AssumeRoleWithWebIdentityResult>
                <SubjectFromWebIdentityToken>amzn1.account.AF6RHO7KZU5XRVQJGXK6HB56KR2A</SubjectFromWebIdentityToken>
                <Audience>client.5498841531868486423.1548@apps.example.com</Audience>
                <AssumedRoleUser>
                  <Arn>arn:aws:sts::123456789012:assumed-role/FederatedWebIdentityRole/app1</Arn>
                  <AssumedRoleId>AROACLKWSDQRAOEXAMPLE:app1</AssumedRoleId>
                </AssumedRoleUser>
                <Credentials>
                  <SessionToken>AQoDYXdzEE0a8ANXXXXXXXXNO1ewxE5TijQyp+IEXAMPLE</SessionToken>
                  <SecretAccessKey>wJalrXUtnFEMI/K7MDENG/bPxRfiCYzEXAMPLEKEY</SecretAccessKey>
                  <Expiration>2014-10-24T23:00:23Z</Expiration>
                  <AccessKeyId>ASgeIAIOSFODNN7EXAMPLE</AccessKeyId>
                </Credentials>
                <SourceIdentity>SourceIdentityValue</SourceIdentity>
                <Provider>www.amazon.com</Provider>
              </AssumeRoleWithWebIdentityResult>
              <ResponseMetadata>
                <RequestId>ad4156e9-bce1-11e2-82e6-6b6efEXAMPLE</RequestId>
              </ResponseMetadata>
            </AssumeRoleWithWebIdentityResponse>
            """);

        var result = response.AssumeRoleWithWebIdentityResult;

        Assert.Equal("amzn1.account.AF6RHO7KZU5XRVQJGXK6HB56KR2A",       result.SubjectFromWebIdentityToken);
        Assert.Equal("client.5498841531868486423.1548@apps.example.com", result.Audience);
        Assert.Equal("AROACLKWSDQRAOEXAMPLE:app1",                       result.AssumedRoleUser.AssumedRoleId);
        Assert.Equal("wJalrXUtnFEMI/K7MDENG/bPxRfiCYzEXAMPLEKEY",        result.Credentials.SecretAccessKey);
        Assert.Equal("www.amazon.com",                                   result.Provider);
    }
}
