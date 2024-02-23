using System.Text.Json;

using Amazon.Kms.Serialization;
using Amazon.Kms.Tests;

namespace Amazon.Kms.Actions.Tests;

public class GenerateDataKeyRequestTests
{
    [Fact]
    public void CanSerialize()
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
            """, JsonSerializer.Serialize(request, JSO.Indented));
    }

    [Fact]
    public void CanSerializePartialRequest()
    {
        var request = new GenerateDataKeyRequest {
            KeyId = "1",
            KeySpec = KeySpec.AES_128
        };

        Assert.Equal("""{"KeyId":"1","KeySpec":"AES_128"}"""u8, JsonSerializer.SerializeToUtf8Bytes(request, KmsSerializerContext.Default.GenerateDataKeyRequest));
    }

    [Fact]
    public void CanSerializeNumberOfBytes()
    {
        var request = new GenerateDataKeyRequest {
            KeyId = "1",
            NumberOfBytes = 128
        };

        Assert.Equal("""{"KeyId":"1","NumberOfBytes":128}"""u8, JsonSerializer.SerializeToUtf8Bytes(request, KmsSerializerContext.Default.GenerateDataKeyRequest));
    }

    [Fact]
    public void CanSerialize2()
    {
        var request = new GenerateDataKeyRequest {
            KeyId = "1",
            KeySpec = KeySpec.AES_128,
            EncryptionContext = new Dictionary<string, string> {
                { "a", "b" }
            }
        };

        Assert.Equal("""{"KeyId":"1","EncryptionContext":{"a":"b"},"KeySpec":"AES_128"}"""u8, JsonSerializer.SerializeToUtf8Bytes(request, KmsSerializerContext.Default.GenerateDataKeyRequest));
    }
}