using System.Text.Json;

namespace Amazon.Kms.Tests;

public class GenerateDataKeyRequestTests
{
    [Fact]
    public void Serialize1()
    {
        var request = new GenerateDataKeyRequest {
            KeyId = "1",
            KeySpec = KeySpec.AES_128,
            NumberOfBytes = 128 // 128, 256, 512, and 1024 
        };

        Assert.Equal(
            """
            {
              "KeyId": "1",
              "KeySpec": "AES_128",
              "NumberOfBytes": 128
            }
            """, JsonSerializer.Serialize(request, JSO.Default));
    }

    [Fact]
    public void Serialize2()
    {
        var request = new GenerateDataKeyRequest {
            KeyId = "1",
            KeySpec = KeySpec.AES_128
        };

        Assert.Equal(
            """
            {
              "KeyId": "1",
              "KeySpec": "AES_128"
            }
            """, JsonSerializer.Serialize(request, JSO.Default));
    }
}