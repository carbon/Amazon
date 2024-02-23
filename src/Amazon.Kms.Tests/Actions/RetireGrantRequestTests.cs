using System.Text.Json;

using Amazon.Kms.Serialization;

namespace Amazon.Kms.Actions.Tests;

public class RetireGrantRequestTests
{
    [Fact]
    public void CanSerialize()
    {
        var request = new RetireGrantRequest("grant-token");

        Assert.Equal(
            """
            {"GrantToken":"grant-token"}
            """u8, JsonSerializer.SerializeToUtf8Bytes(request, KmsSerializerContext.Default.RetireGrantRequest));
    }

    [Fact]
    public void CanSerialize2()
    {
        var request = new RetireGrantRequest(
            "arn:aws:kms:us-east-2:444455556666:key/1234abcd-12ab-34cd-56ef-1234567890ab",
            "1ea8e6c7d4d49ecf7e4461c792f6a27651d7ff0ee13a724c19e730337faa26b1"
        );

        Assert.Equal(
            """
            {"KeyId":"arn:aws:kms:us-east-2:444455556666:key/1234abcd-12ab-34cd-56ef-1234567890ab","GrantId":"1ea8e6c7d4d49ecf7e4461c792f6a27651d7ff0ee13a724c19e730337faa26b1"}
            """u8, JsonSerializer.SerializeToUtf8Bytes(request, KmsSerializerContext.Default.RetireGrantRequest));
    }
}

// https://docs.aws.amazon.com/kms/latest/APIReference/API_RetireGrant.html