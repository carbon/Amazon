using System.Buffers;

namespace Amazon.Cryptography.Tests;

public class EncryptionContextTests
{
    [Fact]
    public void CanConstruct()
    {
        var context = new EncryptionContext() {
            { "accountId"u8, "1"u8 }
        };

        var writer = new ArrayBufferWriter<byte>();

        context.Serialize(writer);

        Assert.Equal("AAEACWFjY291bnRJZAABMQ==", Convert.ToBase64String(writer.WrittenSpan));

        var b = EncryptionContext.Deserialize(writer.WrittenSpan);

        Assert.Single(context);
        Assert.Equal("accountId", context.GetAt(0).Key);
        Assert.Equal("1", context.GetAt(0).Value);
    }
}
