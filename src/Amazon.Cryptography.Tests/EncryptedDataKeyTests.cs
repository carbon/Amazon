using System.Buffers;

using Amazon.Cryptography.Buffers;

namespace Amazon.Cryptography.Tests;

public class EncryptedDataKeyTests
{
    [Fact]
    public void CanConstruct()
    {
        var context = new EncryptedDataKey("aws-kms"u8, "arn:aws:kms:us-west-2:658956600833:key/b3537ef1-d8dc-4780-9f5a-55776cbb2f7f"u8, [1, 2, 3]);

        var writer = new ArrayBufferWriter<byte>();

        context.WriteTo(writer);

        Assert.Equal("AAdhd3Mta21zAEthcm46YXdzOmttczp1cy13ZXN0LTI6NjU4OTU2NjAwODMzOmtleS9iMzUzN2VmMS1kOGRjLTQ3ODAtOWY1YS01NTc3NmNiYjJmN2YAAwECAw==", Convert.ToBase64String(writer.WrittenSpan));

        var buffer = new BufferReader(writer.WrittenSpan);

        Assert.Equal(context, EncryptedDataKey.Read(ref buffer));
    }
}
