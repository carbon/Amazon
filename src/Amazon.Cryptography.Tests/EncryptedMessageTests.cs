using System.Buffers;

namespace Amazon.Cryptography.Tests;

public class EncryptedMessageTests
{
    [Fact]
    public void CanDeserialize()
    {
        // via https://docs.aws.amazon.com/encryption-sdk/latest/developer-guide/message-format-examples.html

        var messageBytes = DocsExample.GetBytes();

        var message = EncryptedMessage.Parse(messageBytes);

        Assert.Equal(AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY_ECDSA_P384, message.Header.AlgorithmId);

        Assert.Equal(4, message.Header.EncryptionContext.Count);

        Assert.Equal(2, message.Header.ContentType); // framed
        Assert.Equal(4096u, message.Header.FrameLength);

        Assert.Equal("EidH6yHf45s4Yxxhf61zQMxiGjAyoRzDIW0CBP0UhFk=", Convert.ToBase64String(message.Header.MessageId));

        Assert.Single(message.Header.EncryptedDataKeys);

        Assert.Equal("aws-kms", message.Header.EncryptedDataKeys[0].ProviderId);
        Assert.Equal("arn:aws:kms:us-west-2:658956600833:key/b3537ef1-d8dc-4780-9f5a-55776cbb2f7f", message.Header.EncryptedDataKeys[0].ProviderInfo);

        Assert.Single(message.Frames);

        Assert.Equal("Bc0DWynVSZ1Fh1cLh1Aq/mNPeyzD3yqpiKEBBUosdoc=", Convert.ToBase64String(message.Header.AlgorithmSuiteData!));
        Assert.Equal("dssznyU2dB9ZocICTyWUqw==", Convert.ToBase64String(message.Header.AuthenticationTag!));

        Assert.NotNull(message.Signature);
        Assert.Equal("MGUCMCoWR62YhnklwXEuj63nCz8qK8O4UOuR71bP3RiWfZHYQtkrrzV7ukj2Nseghpyt4gIxAKquEtCPigr+heUFSAMRDJ7YEbLgisSgUqkHQhfqOwG2YFNKySG/CR0SNlfisJNovQ==", Convert.ToBase64String(message.Signature));

        var writer = new ArrayBufferWriter<byte>();

        message.WriteTo(writer);

        Assert.Equal(messageBytes.AsSpan(), writer.WrittenSpan);
    }
}