using Amazon.Sts.Serialization;

namespace Amazon.Sts.Responses.Tests;

public class AssumeRoleResponseTests
{
    [Fact]
    public void CanDeserialize()
    {
        var response = StsXmlSerializer<AssumeRoleResponse>.Deserialize(
            """
            <AssumeRoleResponse xmlns="https://sts.amazonaws.com/doc/2011-06-15/">
              <AssumeRoleResult>
                <SourceIdentity>Alice</SourceIdentity>
                <AssumedRoleUser>
                  <Arn>arn:aws:sts::123456789012:assumed-role/demo/TestAR</Arn>
                  <AssumedRoleId>ARO123EXAMPLE123:TestAR</AssumedRoleId>
                </AssumedRoleUser>
                <Credentials>
                  <AccessKeyId>ASIAIOSFODNN7EXAMPLE</AccessKeyId>
                  <SecretAccessKey>wJalrXUtnFEMI/K7MDENG/bPxRfiCYzEXAMPLEKEY</SecretAccessKey>
                  <SessionToken>
                   AQoDYXdzEPT//////////wEXAMPLEtc764bNrC9SAPBSM22wDOk4x4HIZ8j4FZTwdQW
                   LWsKWHGBuFqwAeMicRXmxfpSPfIeoIYRqTflfKD8YUuwthAx7mSEI/qkPpKPi/kMcGd
                   QrmGdeehM4IC1NtBmUpp2wUE8phUZampKsburEDy0KPkyQDYwT7WZ0wq5VSXDvp75YU
                   9HFvlRd8Tx6q6fE8YQcHNVXAkiY9q6d+xo0rKwT38xVqr7ZD0u0iPPkUL64lIZbqBAz
                   +scqKmlzm8FDrypNC9Yjc8fPOLn9FX9KSYvKTr4rvx3iSIlTJabIQwj2ICCR/oLxBA==
                  </SessionToken>
                  <Expiration>2019-11-09T13:34:41Z</Expiration>
                </Credentials>
                <PackedPolicySize>6</PackedPolicySize>
              </AssumeRoleResult>
              <ResponseMetadata>
                <RequestId>c6104cbe-af31-11e0-8154-cbc7ccf896c7</RequestId>
              </ResponseMetadata>
            </AssumeRoleResponse>
            """u8.ToArray());

        var result = response.AssumeRoleResult;

        Assert.Equal("Alice",                                              result.SourceIdentity);
        Assert.Equal("arn:aws:sts::123456789012:assumed-role/demo/TestAR", result.AssumedRoleUser.Arn);
        Assert.Equal(6,                                                    result.PackedPolicySize);

        var credentials = result.Credentials;

        Assert.Equal("ASIAIOSFODNN7EXAMPLE",                                   credentials.AccessKeyId);
        Assert.Equal("wJalrXUtnFEMI/K7MDENG/bPxRfiCYzEXAMPLEKEY",              credentials.SecretAccessKey);
        Assert.StartsWith("AQoDYXdzEPT",                                       credentials.SessionToken.Trim());
        Assert.Equal(new DateTime(2019, 11, 09, 13, 34, 41, DateTimeKind.Utc), credentials.Expiration);
    }
}