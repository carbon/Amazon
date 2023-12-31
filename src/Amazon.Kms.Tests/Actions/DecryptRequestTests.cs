using System.Text.Json;

using Amazon.Kms.Serialization;

namespace Amazon.Kms.Actions.Tests;

public class DecryptRequestTests
{
    [Fact]
    public void CanConstruct()
    {
        var data = "test"u8.ToArray();
        var context = new Dictionary<string, string>();
        var request = new DecryptRequest("abc", data, context);

        Assert.Equal("abc", request.KeyId);
        Assert.Null(request.GrantTokens);
        Assert.Equal(data, request.CiphertextBlob);
        Assert.Equal(context, request.EncryptionContext);
    }

    [Fact]
    public void CanSerialize()
    {
        var ciphertext = "test"u8.ToArray();
        var request = new DecryptRequest("abc", ciphertext, null);

        Assert.Equal("abc", request.KeyId);
        Assert.Equal(ciphertext, request.CiphertextBlob);

        Assert.Equal(
            """
            {"KeyId":"abc","CiphertextBlob":"dGVzdA=="}
            """, JsonSerializer.Serialize(request, KmsSerializerContext.Default.DecryptRequest));
    }
}