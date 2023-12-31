using System.Text.Json;

using Amazon.Kms.Serialization;

namespace Amazon.Kms.Actions.Tests;

public class EncryptRequestTests
{
    [Fact]
    public void CanSerialize()
    {
        var request = new EncryptRequest(
            keyId: "1",
            plaintext: "applesauce"u8.ToArray(),
            context: new Dictionary<string, string> {
                            { "user", "1" }
                        }
        );

        Assert.Equal(
            """
            {"EncryptionContext":{"user":"1"},"KeyId":"1","Plaintext":"YXBwbGVzYXVjZQ=="}
            """u8, JsonSerializer.SerializeToUtf8Bytes(request, KmsSerializerContext.Default.EncryptRequest));
    }
}