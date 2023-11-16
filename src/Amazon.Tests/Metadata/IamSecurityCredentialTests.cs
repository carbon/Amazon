using System.Text.Json;

using Amazon.Metadata.Serialization;

namespace Amazon.Metadata.Tests;

public class IamSecurityCredentialTests
{
    [Fact]
    public void CanDeserialize()
    {
        var credential = JsonSerializer.Deserialize<IamSecurityCredentials>(
            """
            {
              "Code" : "Success",
              "LastUpdated" : "2012-04-26T16:39:16Z",
              "Type" : "AWS-HMAC",
              "AccessKeyId" : "ASIAIOSFODNN7EXAMPLE",
              "SecretAccessKey" : "wJalrXUtnFEMI/K7MDENG/bPxRfiCYEXAMPLEKEY",
              "Token" : "token",
              "Expiration" : "2017-05-17T15:09:54Z"
            }
            """);

        Assert.NotNull(credential);
        Assert.Equal("Success", credential.Code);
        Assert.Equal("AWS-HMAC", credential.Type);
        Assert.Equal("ASIAIOSFODNN7EXAMPLE", credential.AccessKeyId);
        Assert.Equal("wJalrXUtnFEMI/K7MDENG/bPxRfiCYEXAMPLEKEY", credential.SecretAccessKey);
        Assert.Equal("token", credential.Token);
        Assert.Equal(new DateTime(2017, 05, 17, 15, 09, 54, DateTimeKind.Utc), credential.Expiration);
    }

    [Fact]
    public void CanDeserializeWithJsonContext()
    {
        var credential = JsonSerializer.Deserialize(
            """
            {
              "Code" : "Success",
              "LastUpdated" : "2012-04-26T16:39:16Z",
              "Type" : "AWS-HMAC",
              "AccessKeyId" : "ASIAIOSFODNN7EXAMPLE",
              "SecretAccessKey" : "wJalrXUtnFEMI/K7MDENG/bPxRfiCYEXAMPLEKEY",
              "Token" : "token",
              "Expiration" : "2017-05-17T15:09:54Z"
            }
            """, MetadataSerializerContext.Default.IamSecurityCredentials);

        Assert.NotNull(credential);
        Assert.Equal("Success", credential.Code);
        Assert.Equal("AWS-HMAC", credential.Type);
        Assert.Equal("ASIAIOSFODNN7EXAMPLE", credential.AccessKeyId);
        Assert.Equal("wJalrXUtnFEMI/K7MDENG/bPxRfiCYEXAMPLEKEY", credential.SecretAccessKey);
        Assert.Equal("token", credential.Token);
        Assert.Equal(new DateTime(2017, 05, 17, 15, 09, 54, DateTimeKind.Utc), credential.Expiration);
    }
}
