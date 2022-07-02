using System.Text;
using System.Text.Json;

namespace Amazon.Kms.Tests;

public class DecryptRequestTests
{
    [Fact]
    public void Construct()
    {
        var data = Encoding.UTF8.GetBytes("test");
        var context = new Dictionary<string, string>();
        var request = new DecryptRequest("abc", data, context);

        Assert.Equal("abc", request.KeyId);
        Assert.Null(request.GrantTokens);
        Assert.Equal(data, request.CiphertextBlob);
        Assert.Equal(context, request.EncryptionContext);
    }

    [Fact]
    public void Serialize()
    {
        var data = Encoding.UTF8.GetBytes("test");
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