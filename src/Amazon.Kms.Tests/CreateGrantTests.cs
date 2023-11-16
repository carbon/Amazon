using System.Text.Json;

using Amazon.Kms.Serialization;

namespace Amazon.Kms.Tests;

public class CreateGrantTests
{
    [Fact]
    public void CanSerialize()
    {
        var request = new CreateGrantRequest {
            KeyId = "key",
            GranteePrincipal = "principle",
            Operations = [KmsOperations.Decrypt],
            Constraints = new GrantConstraints {
                EncryptionContextEquals = new Dictionary<string, string> {
                    { "vault", "master" }
                }
            }
        };

        Assert.Equal(
            """
            {
              "KeyId": "key",
              "GranteePrincipal": "principle",
              "Operations": [
                "Decrypt"
              ],
              "Constraints": {
                "EncryptionContextEquals": {
                  "vault": "master"
                }
              }
            }
            """, JsonSerializer.Serialize(request, JSO.Indented));


        Assert.Equal(
            """
            {"KeyId":"key","GranteePrincipal":"principle","Operations":["Decrypt"],"Constraints":{"EncryptionContextEquals":{"vault":"master"}}}
            """, JsonSerializer.Serialize(request, KmsSerializerContext.Default.CreateGrantRequest));
    }
}
