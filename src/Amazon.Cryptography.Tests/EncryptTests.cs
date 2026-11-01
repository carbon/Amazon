using System.Buffers;
using System.Security.Cryptography;

using Amazon.Cryptography.Algorithms;

namespace Amazon.Cryptography.Tests;

// The library has no decrypt path, so these tests decrypt frames directly with AesGcm, using the documented frame AAD
public class EncryptTests
{
    private const int FrameLength = 4096;

    private static readonly byte[] s_key = Enumerable.Range(0, 32).Select(i => (byte)i).ToArray();

    private static EncryptionContext Context() => new() { { "purpose"u8, "test"u8 } };

    private static byte[] Plaintext(int length) => Enumerable.Range(0, length).Select(i => (byte)(i * 7)).ToArray();

    [Theory]
    [InlineData(AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY, 1, 1)]
    [InlineData(AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY, FrameLength - 1, 1)]
    [InlineData(AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY, FrameLength, 1)]
    [InlineData(AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY, FrameLength + 1, 2)]
    [InlineData(AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY, FrameLength * 2, 2)]
    [InlineData(AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY_ECDSA_P384, 1, 1)]
    [InlineData(AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY_ECDSA_P384, FrameLength, 1)]
    [InlineData(AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY_ECDSA_P384, (FrameLength * 2) + 5, 3)]
    public void Frames_RoundTrip_AcrossFrameBoundaries(AlgorithmSuiteId suite, int length, int expectedFrames)
    {
        var plaintext = Plaintext(length);
        var context = Context();

        var message = MessageBytes.Encrypt(suite, s_key, plaintext, context);

        Assert.Equal(expectedFrames, message.Frames.Count);

        var decrypted = new List<byte>();

        for (int i = 0; i < message.Frames.Count; i++)
        {
            var frame = message.Frames[i];

            Assert.Equal((uint)(i + 1), frame.SequenceNumber);
            Assert.Equal(i == message.Frames.Count - 1, frame.IsFinal);
            Assert.Equal(MessageBytes.Iv(frame.SequenceNumber), frame.IV);
            Assert.Equal(16, frame.AuthenticationTag.Length);

            if (!frame.IsFinal)
            {
                Assert.Equal(FrameLength, frame.EncryptedContent.Length);
            }

            decrypted.AddRange(MessageBytes.DecryptFrame(suite, s_key, message.Header.MessageId, frame, MessageBytes.SpecFrameAad(message.Header.MessageId, frame)));
        }

        Assert.Equal(plaintext, decrypted);
    }

    [Theory]
    [InlineData(AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY)]
    [InlineData(AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY_ECDSA_P384)]
    public void Ciphertext_DoesNotContainPlaintext(AlgorithmSuiteId suite)
    {
        var plaintext = new byte[64];

        Array.Fill(plaintext, (byte)'A');

        var message = MessageBytes.Encrypt(suite, s_key, plaintext);

        Assert.NotEqual(plaintext, message.Frames[0].EncryptedContent);
        Assert.Equal(plaintext.Length, message.Frames[0].EncryptedContent.Length);
    }

    [Theory]
    [InlineData(AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY)]
    [InlineData(AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY_ECDSA_P384)]
    public void SameKeyAndPlaintext_UseFreshMessageIdAndCiphertext(AlgorithmSuiteId suite)
    {
        var plaintext = Plaintext(32);

        var a = MessageBytes.Encrypt(suite, s_key, plaintext);
        var b = MessageBytes.Encrypt(suite, s_key, plaintext);

        Assert.Equal(32, a.Header.MessageId.Length);
        Assert.NotEqual(a.Header.MessageId, b.Header.MessageId);

        // The frame IVs repeat by design; a fresh message id is what makes the derived key, and so the (key, IV) pair, unique
        Assert.Equal(a.Frames[0].IV, b.Frames[0].IV);
        Assert.NotEqual(a.Frames[0].EncryptedContent, b.Frames[0].EncryptedContent);
    }

    [Fact]
    public void FrameIvs_AreUniqueWithinMessage()
    {
        var message = MessageBytes.Encrypt(AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY, s_key, Plaintext((FrameLength * 3) + 1));

        Assert.Equal(4, message.Frames.Select(f => Convert.ToHexString(f.IV)).Distinct().Count());
    }

    [Theory]
    [InlineData(AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY)]
    [InlineData(AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY_ECDSA_P384)]
    public void TamperedContent_IsRejected(AlgorithmSuiteId suite)
    {
        var context = Context();
        var frame = MessageBytes.Encrypt(suite, s_key, Plaintext(32), context);
        var original = frame.Frames[0];

        for (int i = 0; i < original.EncryptedContent.Length; i++)
        {
            var content = (byte[])original.EncryptedContent.Clone();

            content[i] ^= 0x01;

            AssertRejected(suite, s_key, frame.Header.MessageId, Copy(original, encryptedContent: content));
        }
    }

    [Theory]
    [InlineData(AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY)]
    [InlineData(AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY_ECDSA_P384)]
    public void TamperedTag_IsRejected(AlgorithmSuiteId suite)
    {
        var context = Context();
        var message = MessageBytes.Encrypt(suite, s_key, Plaintext(32), context);
        var original = message.Frames[0];

        for (int i = 0; i < original.AuthenticationTag.Length; i++)
        {
            var tag = (byte[])original.AuthenticationTag.Clone();

            tag[i] ^= 0x80;

            AssertRejected(suite, s_key, message.Header.MessageId, Copy(original, tag: tag));
        }
    }

    [Fact]
    public void TamperedIv_IsRejected()
    {
        const AlgorithmSuiteId suite = AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY;

        var context = Context();
        var message = MessageBytes.Encrypt(suite, s_key, Plaintext(32), context);

        AssertRejected(suite, s_key, message.Header.MessageId, Copy(message.Frames[0], iv: MessageBytes.Iv(2)));
    }

    [Fact]
    public void ReorderedFrames_AreRejected()
    {
        const AlgorithmSuiteId suite = AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY;

        var context = Context();
        var message = MessageBytes.Encrypt(suite, s_key, Plaintext(FrameLength * 2), context);

        // The second frame's ciphertext presented under the first frame's IV
        var swapped = Copy(message.Frames[1], iv: message.Frames[0].IV);

        AssertRejected(suite, s_key, message.Header.MessageId, swapped);
    }

    [Fact]
    public void WrongDataKey_IsRejected()
    {
        const AlgorithmSuiteId suite = AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY;

        var context = Context();
        var message = MessageBytes.Encrypt(suite, s_key, Plaintext(32), context);

        var wrongKey = (byte[])s_key.Clone();

        wrongKey[0] ^= 1;

        AssertRejected(suite, wrongKey, message.Header.MessageId, message.Frames[0]);
    }

    [Fact]
    public void FrameFromAnotherMessage_IsRejected_EvenWithSameDataKey()
    {
        const AlgorithmSuiteId suite = AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY;

        var context = Context();
        var a = MessageBytes.Encrypt(suite, s_key, Plaintext(32), context);
        var b = MessageBytes.Encrypt(suite, s_key, Plaintext(32), context);

        AssertRejected(suite, s_key, b.Header.MessageId, a.Frames[0]);
    }

    [Fact]
    public void SuitesDeriveDifferentKeys_SoFramesDoNotCrossSuites()
    {
        var context = Context();
        var message = MessageBytes.Encrypt(AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY, s_key, Plaintext(32), context);

        AssertRejected(AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY_ECDSA_P384, s_key, message.Header.MessageId, message.Frames[0]);
    }

    [Theory]
    [InlineData(AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY, 0)]
    [InlineData(AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY, 16)]
    [InlineData(AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY, 31)]
    [InlineData(AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY_ECDSA_P384, 33)]
    public void WrongRawKeyLength_IsRejected(AlgorithmSuiteId suite, int length)
    {
        var e = Assert.Throws<ArgumentException>(() => MessageBytes.Encrypt(suite, new byte[length], [1]));

        Assert.Equal("key", e.ParamName);
    }

    [Fact]
    public void Header_DescribesMessage()
    {
        var context = Context();
        var message = MessageBytes.Encrypt(AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY, s_key, [1], context);

        Assert.Equal(2, message.Header.Version);
        Assert.Equal(AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY, message.Header.AlgorithmId);
        Assert.Equal(2, message.Header.ContentType);
        Assert.Equal((uint)FrameLength, message.Header.FrameLength);
        Assert.Same(context, message.Header.EncryptionContext);
        Assert.Equal("fake", message.Header.EncryptedDataKeys[0].ProviderId);
        Assert.Equal("fake-key", message.Header.EncryptedDataKeys[0].ProviderInfo);
    }

    [Theory]
    [InlineData(AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY)]
    [InlineData(AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY_ECDSA_P384)]
    public void Header_CarriesKeyCommitment(AlgorithmSuiteId suite)
    {
        var message = MessageBytes.Encrypt(suite, s_key, [1]);
        var expected = new byte[32];

        AES256_GCM_HKDF_SHA512_COMMIT_KEY.Default.CalculateCommitmentKey(s_key, message.Header.MessageId, expected);

        Assert.Equal(expected, message.Header.AlgorithmSuiteData);
    }

    [Theory]
    [InlineData(AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY)]
    [InlineData(AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY_ECDSA_P384)]
    public void Header_AuthenticationTagVerifies(AlgorithmSuiteId suite)
    {
        var message = MessageBytes.Encrypt(suite, s_key, [1], Context());

        Assert.Equal(16, message.Header.AuthenticationTag.Length);

        var writer = new ArrayBufferWriter<byte>();

        message.Header.WriteTo(writer);

        var headerBody = writer.WrittenSpan[..^16].ToArray();
        var derivedKey = new byte[32];

        AlgorithmSuite.Get(suite).DeriveKey(s_key, message.Header.MessageId, derivedKey);

        using var aes = new AesGcm(derivedKey, 16);

        aes.Decrypt(new byte[12], Array.Empty<byte>(), message.Header.AuthenticationTag, Array.Empty<byte>(), headerBody);
    }

    [Theory]
    [InlineData(AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY)]
    [InlineData(AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY_ECDSA_P384)]
    public void FrameAad_FollowsDocumentedFormat(AlgorithmSuiteId suite)
    {
        var plaintext = Plaintext(FrameLength + 1);
        var message = MessageBytes.Encrypt(suite, s_key, plaintext, Context());

        var first = MessageBytes.DecryptFrame(suite, s_key, message.Header.MessageId, message.Frames[0], MessageBytes.SpecFrameAad(message.Header.MessageId, message.Frames[0]));
        var last = MessageBytes.DecryptFrame(suite, s_key, message.Header.MessageId, message.Frames[1], MessageBytes.SpecFrameAad(message.Header.MessageId, message.Frames[1]));

        Assert.Equal(plaintext, first.Concat(last));
    }

    [Theory]
    [InlineData(AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY)]
    [InlineData(AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY_ECDSA_P384)]
    public void EmptyPlaintext_ProducesEmptyFinalFrame(AlgorithmSuiteId suite)
    {
        var message = MessageBytes.Encrypt(suite, s_key, []);

        var frame = Assert.Single(message.Frames);

        Assert.True(frame.IsFinal);
        Assert.Equal(1u, frame.SequenceNumber);
        Assert.Empty(frame.EncryptedContent);
        Assert.Equal(16, frame.AuthenticationTag.Length);
    }

    [Fact]
    public void Header_CarriesEncryptedDataKey()
    {
        var message = MessageBytes.Encrypt(AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY, s_key, [1]);

        Assert.Equal(MessageBytes.FakeEncryptedDataKey, message.Header.EncryptedDataKeys[0].Ciphertext);
    }

    [Theory]
    [InlineData(AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY)]
    [InlineData(AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY_ECDSA_P384)]
    public void EncryptedMessage_SurvivesWriteAndParse(AlgorithmSuiteId suite)
    {
        var message = MessageBytes.Encrypt(suite, s_key, Plaintext(FrameLength + 1), Context());

        var writer = new ArrayBufferWriter<byte>();

        message.WriteTo(writer);

        var parsed = EncryptedMessage.Parse(writer.WrittenSpan);

        Assert.Equal(message.Header.MessageId, parsed.Header.MessageId);
        Assert.Equal(2, parsed.Frames.Count);
        Assert.Equal(message.Frames[1].EncryptedContent, parsed.Frames[1].EncryptedContent);
        Assert.Equal(message.Signature, parsed.Signature);

        var rewritten = new ArrayBufferWriter<byte>();

        parsed.WriteTo(rewritten);

        Assert.Equal(writer.WrittenSpan, rewritten.WrittenSpan);
    }

    [Fact]
    public void SigningSuite_ProducesSignatureAndPublicKey()
    {
        var message = MessageBytes.Encrypt(AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY_ECDSA_P384, s_key, [1]);

        Assert.NotNull(message.Signature);
        Assert.Contains(message.Header.EncryptionContext, entry => entry.Key.ToString() == "aws-crypto-public-key");
    }

    [Fact]
    public void RequestForDifferentSuite_IsRejected()
    {
        var e = Assert.Throws<ArgumentException>(() => {
            var key = new DataKey { RawKey = s_key, Ciphertext = [1], ProviderId = "fake"u8, ProviderContext = "fake-key"u8 };

            return AES256_GCM_HKDF_SHA512_COMMIT_KEY.Default.Encrypt(key, new EncryptMessageRequest {
                AlgorithmId = AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY_ECDSA_P384,
                EncryptionContext = [],
                Plaintext = [1]
            });
        });

        Assert.Equal("request", e.ParamName);
    }

    [Fact]
    public void FrameRelabelledAsFinal_IsRejected()
    {
        const AlgorithmSuiteId suite = AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY;

        var message = MessageBytes.Encrypt(suite, s_key, Plaintext(FrameLength + 1));

        // Truncation: drop the real final frame and present the first frame as the last
        AssertRejected(suite, s_key, message.Header.MessageId, Copy(message.Frames[0], isFinal: true));
    }

    [Fact]
    public void TamperedHeader_FailsTagVerification()
    {
        const AlgorithmSuiteId suite = AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY;

        var message = MessageBytes.Encrypt(suite, s_key, [1], Context());

        var tampered = message.Header with { EncryptionContext = new EncryptionContext { { "purpose"u8, "other"u8 } } };

        var writer = new ArrayBufferWriter<byte>();

        tampered.WriteTo(writer);

        var derivedKey = new byte[32];

        AlgorithmSuite.Get(suite).DeriveKey(s_key, message.Header.MessageId, derivedKey);

        using var aes = new AesGcm(derivedKey, 16);

        var headerBody = writer.WrittenSpan[..^16].ToArray();

        Assert.ThrowsAny<CryptographicException>(() => aes.Decrypt(new byte[12], Array.Empty<byte>(), message.Header.AuthenticationTag, Array.Empty<byte>(), headerBody));
    }

    [Fact]
    public void Signature_VerifiesOverHeaderAndBody_WithPublishedKey()
    {
        var context = Context();
        var message = MessageBytes.Encrypt(AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY_ECDSA_P384, s_key, Plaintext(FrameLength + 1), context);

        var writer = new ArrayBufferWriter<byte>();

        message.Header.WriteTo(writer);

        foreach (var frame in message.Frames)
        {
            frame.WriteTo(writer);
        }

        var signed = writer.WrittenSpan.ToArray();

        string publicKey = message.Header.EncryptionContext["aws-crypto-public-key"u8.ToArray()];

        using var ecdsa = MessageBytes.ImportCompressedP384(Convert.FromBase64String(publicKey));

        Assert.True(ecdsa.VerifyData(signed, message.Signature!, HashAlgorithmName.SHA384, DSASignatureFormat.Rfc3279DerSequence));

        signed[^1] ^= 1;

        Assert.False(ecdsa.VerifyData(signed, message.Signature!, HashAlgorithmName.SHA384, DSASignatureFormat.Rfc3279DerSequence));

        // The caller's context is left as it was
        Assert.Single(context);
    }

    [Fact]
    public void UnsignedSuite_HasNoSignatureOrPublicKey()
    {
        var message = MessageBytes.Encrypt(AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY, s_key, [1]);

        Assert.Null(message.Signature);
        Assert.Empty(message.Header.EncryptionContext);
    }

    [Theory]
    [InlineData(AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY)]
    [InlineData(AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY_ECDSA_P384)]
    public void ReservedContextKey_IsRejected(AlgorithmSuiteId suite)
    {
        var context = new EncryptionContext { { "aws-crypto-public-key"u8, "forged"u8 } };

        var e = Assert.Throws<ArgumentException>(() => MessageBytes.Encrypt(suite, s_key, [1], context));

        Assert.Equal("request", e.ParamName);
    }

    [Theory]
    [InlineData(new byte[] { 0xFF }, new byte[] { 0x31 })]
    [InlineData(new byte[] { 0x61 }, new byte[] { 0xC3 })]
    public void InvalidUtf8InContext_IsRejected(byte[] key, byte[] value)
    {
        var context = new EncryptionContext { { key, value } };

        var e = Assert.Throws<ArgumentException>(() => MessageBytes.Encrypt(AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY, s_key, [1], context));

        Assert.Equal("request", e.ParamName);
    }

    [Fact]
    public void MissingEncryptedDataKey_IsRejected()
    {
        var e = Assert.Throws<ArgumentException>(() => {
            var key = new DataKey { RawKey = s_key, ProviderId = "fake"u8, ProviderContext = "fake-key"u8 };

            return AES256_GCM_HKDF_SHA512_COMMIT_KEY.Default.Encrypt(key, new EncryptMessageRequest {
                AlgorithmId = AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY,
                EncryptionContext = [],
                Plaintext = [1]
            });
        });

        Assert.Equal("key", e.ParamName);
    }

    [Fact]
    public void OversizedContext_IsRejected()
    {
        var context = new EncryptionContext { { "a"u8, new byte[70_000] } };

        Assert.Throws<InvalidOperationException>(() => MessageBytes.Encrypt(AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY, s_key, [1], context));
    }

    private static void AssertRejected(AlgorithmSuiteId suite, byte[] key, byte[] messageId, EncryptedMessageFrame frame)
    {
        Assert.ThrowsAny<CryptographicException>(() => MessageBytes.DecryptFrame(suite, key, messageId, frame, MessageBytes.SpecFrameAad(messageId, frame)));
    }

    private static EncryptedMessageFrame Copy(EncryptedMessageFrame frame, byte[]? iv = null, byte[]? encryptedContent = null, byte[]? tag = null, bool? isFinal = null)
    {
        return new EncryptedMessageFrame {
            SequenceNumber = frame.SequenceNumber,
            IV = iv ?? frame.IV,
            EncryptedContent = encryptedContent ?? frame.EncryptedContent,
            AuthenticationTag = tag ?? frame.AuthenticationTag,
            IsFinal = isFinal ?? frame.IsFinal
        };
    }
}
