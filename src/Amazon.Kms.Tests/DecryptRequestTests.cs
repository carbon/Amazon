using System.Text.Json;

namespace Amazon.Kms.Tests;

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
        var data = "test"u8.ToArray();
        var request = new DecryptRequest("abc", data, null);

        Assert.Equal("abc", request.KeyId);
        Assert.Equal(data, request.CiphertextBlob);

        Assert.Equal(
            """
            {
              "KeyId": "abc",
              "CiphertextBlob": "dGVzdA=="
            }
            """, JsonSerializer.Serialize(request, JSO.Default));
    }
}