using System.Buffers;

namespace Amazon.Cryptography.Tests;

public class EncryptedMessageParseTests
{
    private static readonly byte[] s_docsMessage = DocsExample.GetBytes();

    private const int DocsFooterLength = 2 + 103;

    [Fact]
    public void DocsExample_FinalFrameFields_AreParsed()
    {
        var frame = Assert.Single(EncryptedMessage.Parse(s_docsMessage).Frames);

        Assert.True(frame.IsFinal);
        Assert.Equal(1u, frame.SequenceNumber);
        Assert.Equal(MessageBytes.Iv(1), frame.IV);
        Assert.Equal("fa6e39c6029273993e", Convert.ToHexStringLower(frame.EncryptedContent));
        Assert.Equal("f683a564405d68dbeeb0656cd57c9eb0", Convert.ToHexStringLower(frame.AuthenticationTag));
    }

    [Fact]
    public void AnyTruncation_IsRejected_UpToEndOfBody()
    {
        var bodyEnd = s_docsMessage.Length - DocsFooterLength;

        for (int length = 0; length < bodyEnd; length++)
        {
            var truncated = s_docsMessage[..length];

            Assert.ThrowsAny<Exception>(() => EncryptedMessage.Parse(truncated));
        }
    }

    [Fact]
    public void TruncatedFooter_IsRejected()
    {
        var bodyEnd = s_docsMessage.Length - DocsFooterLength;

        for (int length = bodyEnd + 1; length < s_docsMessage.Length; length++)
        {
            var truncated = s_docsMessage[..length];

            Assert.ThrowsAny<Exception>(() => EncryptedMessage.Parse(truncated));
        }
    }

    [Fact]
    public void SigningSuite_WithoutFooter_IsRejected()
    {
        var stripped = s_docsMessage[..^DocsFooterLength];

        Assert.ThrowsAny<Exception>(() => EncryptedMessage.Parse(stripped));
    }

    [Fact]
    public void TrailingBytes_AfterFooter_AreRejected()
    {
        byte[] extended = [.. s_docsMessage, 0x00];

        Assert.ThrowsAny<Exception>(() => EncryptedMessage.Parse(extended));
    }

    [Fact]
    public void UnsignedSuite_WithFooter_IsRejected()
    {
        var bytes = new MessageBytes { Footer = [0x00, 0x01, 0xAA] }.ToArray();

        Assert.ThrowsAny<Exception>(() => EncryptedMessage.Parse(bytes));
    }

    [Fact]
    public void UnsignedMessage_RoundTrips()
    {
        var bytes = new MessageBytes().ToArray();

        var message = EncryptedMessage.Parse(bytes);

        Assert.Null(message.Signature);

        var writer = new ArrayBufferWriter<byte>();

        message.WriteTo(writer);

        Assert.Equal(bytes.AsSpan(), writer.WrittenSpan);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(3)]
    public void UnsupportedVersion_IsRejected(byte version)
    {
        var bytes = new MessageBytes { Version = version }.ToArray();

        var e = Assert.Throws<InvalidOperationException>(() => EncryptedMessage.Parse(bytes));

        Assert.Contains(version.ToString(), e.Message);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)] // non-framed is only supported for legacy use
    [InlineData(3)]
    public void UnsupportedContentType_IsRejected(byte contentType)
    {
        var bytes = new MessageBytes { ContentType = contentType }.ToArray();

        var e = Assert.ThrowsAny<Exception>(() => EncryptedMessage.Parse(bytes));

        Assert.Contains("ContentType", e.Message);
    }

    [Theory]
    [InlineData(0x0378)] // valid suite, but message format version 1
    [InlineData(0x0014)]
    [InlineData(0xFFFF)]
    public void UnsupportedAlgorithm_IsRejected(ushort algorithmId)
    {
        var bytes = new MessageBytes { AlgorithmId = algorithmId }.ToArray();

        Assert.Throws<NotImplementedException>(() => EncryptedMessage.Parse(bytes));
    }

    [Fact]
    public void UnsupportedAlgorithm_IsRejected_BeforeReadingTheRestOfTheHeader()
    {
        byte[] bytes = [0x02, 0x03, 0x78]; // nothing after the algorithm id

        Assert.Throws<NotImplementedException>(() => EncryptedMessage.Parse(bytes));
    }

    [Fact]
    public void UnsupportedContentType_IsRejected_BeforeReadingFrameLength()
    {
        const int contentTypeOffset = 1 + 2 + 32 + 2 + 2 + 9; // version, algorithm, message id, AAD length, key count, one data key

        var bytes = new MessageBytes { ContentType = 1 }.ToArray()[..(contentTypeOffset + 1)];

        var e = Assert.ThrowsAny<Exception>(() => EncryptedMessage.Parse(bytes));

        Assert.Contains("ContentType", e.Message);
    }

    [Fact]
    public void InvalidUtf8InEncryptionContext_IsRejected()
    {
        var bytes = new MessageBytes { Aad = Convert.FromHexString("0001" + "0001FF" + "000131") }.ToArray();

        Assert.Throws<FormatException>(() => EncryptedMessage.Parse(bytes));
    }

    [Fact]
    public void MultipleFrames_AreParsedInOrder()
    {
        byte[] body = [
            .. MessageBytes.RegularFrame(1, [1, 1, 1, 1]),
            .. MessageBytes.RegularFrame(2, [2, 2, 2, 2]),
            .. MessageBytes.FinalFrame(3, [3])
        ];

        var message = EncryptedMessage.Parse(new MessageBytes { Body = body }.ToArray());

        Assert.Equal(3, message.Frames.Count);
        Assert.Equal([1u, 2u, 3u], message.Frames.Select(f => f.SequenceNumber));
        Assert.Equal([false, false, true], message.Frames.Select(f => f.IsFinal));
        Assert.Equal([1, 1, 1, 1], message.Frames[0].EncryptedContent);
        Assert.Equal(MessageBytes.Iv(2), message.Frames[1].IV);
        Assert.Equal([3], message.Frames[2].EncryptedContent);
    }

    [Fact]
    public void Frames_WriteBackToTheirOriginalBytes()
    {
        var regular = MessageBytes.RegularFrame(1, [1, 1, 1, 1]);
        var final = MessageBytes.FinalFrame(2, [2]);

        var message = EncryptedMessage.Parse(new MessageBytes { Body = [.. regular, .. final] }.ToArray());

        var writer = new ArrayBufferWriter<byte>();

        message.Frames[0].WriteTo(writer);
        message.Frames[1].WriteTo(writer);

        Assert.Equal([.. regular, .. final], writer.WrittenSpan.ToArray());
    }

    [Fact]
    public void EmptyFinalFrame_IsAllowed()
    {
        byte[] body = [
            .. MessageBytes.RegularFrame(1, [1, 1, 1, 1]),
            .. MessageBytes.FinalFrame(2, [])
        ];

        var message = EncryptedMessage.Parse(new MessageBytes { Body = body }.ToArray());

        Assert.Empty(message.Frames[1].EncryptedContent);
    }

    [Fact]
    public void BodyWithoutFinalFrame_IsRejected()
    {
        var bytes = new MessageBytes { Body = MessageBytes.RegularFrame(1, [1, 1, 1, 1]) }.ToArray();

        Assert.ThrowsAny<Exception>(() => EncryptedMessage.Parse(bytes));
    }

    [Fact]
    public void FinalFrameLength_LargerThanRemainingData_IsRejected()
    {
        var bytes = new MessageBytes { Body = MessageBytes.FinalFrame(1, [1, 2, 3], declaredLength: 1000) }.ToArray();

        Assert.ThrowsAny<Exception>(() => EncryptedMessage.Parse(bytes));
    }

    [Theory]
    [InlineData(0x80000000u)]
    [InlineData(uint.MaxValue - 1)]
    public void FinalFrameLength_AboveIntMaxValue_IsRejected(uint length)
    {
        var bytes = new MessageBytes { Body = MessageBytes.FinalFrame(1, [1, 2, 3], declaredLength: length) }.ToArray();

        Assert.ThrowsAny<Exception>(() => EncryptedMessage.Parse(bytes));
    }

    [Fact]
    public void HeaderFrameLength_AboveIntMaxValue_IsRejected()
    {
        var bytes = new MessageBytes { FrameLength = 0x80000000u, Body = MessageBytes.RegularFrame(1, [1]) }.ToArray();

        Assert.ThrowsAny<Exception>(() => EncryptedMessage.Parse(bytes));
    }

    [Fact]
    public void EncryptionContextLength_LargerThanRemainingData_IsRejected()
    {
        var bytes = new MessageBytes().ToArray();

        bytes[35] = 0xFF;
        bytes[36] = 0xFF;

        Assert.ThrowsAny<Exception>(() => EncryptedMessage.Parse(bytes));
    }

    [Fact]
    public void ZeroEncryptedDataKeys_IsRejected()
    {
        var bytes = new MessageBytes { EncryptedDataKeyCount = 0 }.ToArray();

        Assert.ThrowsAny<Exception>(() => EncryptedMessage.Parse(bytes));
    }

    [Fact]
    public void FirstSequenceNumber_MustBeOne()
    {
        var bytes = new MessageBytes { Body = MessageBytes.FinalFrame(2, [1]) }.ToArray();

        Assert.ThrowsAny<Exception>(() => EncryptedMessage.Parse(bytes));
    }

    [Fact]
    public void SequenceNumbers_MustIncrementByOne()
    {
        byte[] body = [
            .. MessageBytes.RegularFrame(1, [1, 1, 1, 1]),
            .. MessageBytes.FinalFrame(3, [1])
        ];

        var bytes = new MessageBytes { Body = body }.ToArray();

        Assert.ThrowsAny<Exception>(() => EncryptedMessage.Parse(bytes));
    }

    [Fact]
    public void FinalFrame_LongerThanFrameLength_IsRejected()
    {
        var bytes = new MessageBytes { FrameLength = 2, Body = MessageBytes.FinalFrame(1, [1, 2, 3]) }.ToArray();

        Assert.ThrowsAny<Exception>(() => EncryptedMessage.Parse(bytes));
    }

    [Fact]
    public void EmptySignature_IsRejected()
    {
        var bytes = new MessageBytes {
            AlgorithmId = (ushort)AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY_ECDSA_P384,
            Footer = [0x00, 0x00]
        }.ToArray();

        Assert.ThrowsAny<FormatException>(() => EncryptedMessage.Parse(bytes));
    }

    [Fact]
    public void MissingHeader_IsRejected()
    {
        Assert.ThrowsAny<Exception>(() => EncryptedMessage.Parse([]));
    }
}
