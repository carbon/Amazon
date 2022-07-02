using System.Text;
using System.Text.Json;

namespace Amazon.Kms.Tests;

public class EncryptRequestTests
{
    [Fact]
    public void CanSerialize()
    {
        var request = new EncryptRequest(
            keyId     : "1",
            plaintext : Encoding.UTF8.GetBytes("applesauce"),
            context   : new Dictionary<string, string> {
                            { "user", "1" }
                        }
        );

        Assert.Equal(
            """
            {
              "EncryptionContext": {
                "user": "1"
              },
              "KeyId": "1",
              "Plaintext": "YXBwbGVzYXVjZQ=="
            }
            """, JsonSerializer.Serialize(request, JSO.Default));
    }
}