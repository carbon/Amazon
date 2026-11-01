using System.Buffers;
using System.Buffers.Binary;
using System.Globalization;
using System.Numerics;
using System.Security.Cryptography;

using Amazon.Cryptography.Algorithms;

namespace Amazon.Cryptography.Tests;

// Builds wire-format messages by hand, independently of the production writers, so parser tests can express malformed input
internal sealed class MessageBytes
{
    public static readonly byte[] FakeEncryptedDataKey = [0xFA, 0xCE];

    public byte Version { get; init; } = 2;

    public ushort AlgorithmId { get; init; } = (ushort)AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY;

    public byte[] Aad { get; init; } = [];

    public ushort EncryptedDataKeyCount { get; init; } = 1;

    public byte ContentType { get; init; } = 2;

    public uint FrameLength { get; init; } = 4;

    public byte[] Body { get; init; } = FinalFrame(1, [1, 2, 3]);

    public byte[] Footer { get; init; } = [];

    public byte[] ToArray()
    {
        var writer = new ArrayBufferWriter<byte>();

        writer.Write([Version]);
        WriteUInt16(writer, AlgorithmId);
        writer.Write(new byte[32]); // message id
        WriteUInt16(writer, (ushort)Aad.Length);
        writer.Write(Aad);
        WriteUInt16(writer, EncryptedDataKeyCount);

        for (int i = 0; i < EncryptedDataKeyCount; i++)
        {
            WriteUInt16(writer, 1);
            writer.Write("p"u8);
            WriteUInt16(writer, 1);
            writer.Write("i"u8);
            WriteUInt16(writer, 1);
            writer.Write([(byte)0xEE]);
        }

        writer.Write([ContentType]);
        WriteUInt32(writer, FrameLength);
        writer.Write(new byte[32]); // algorithm suite data
        writer.Write(new byte[16]); // header authentication tag
        writer.Write(Body);
        writer.Write(Footer);

        return writer.WrittenSpan.ToArray();
    }

    public static byte[] RegularFrame(uint sequenceNumber, byte[] content)
    {
        var writer = new ArrayBufferWriter<byte>();

        WriteUInt32(writer, sequenceNumber);
        writer.Write(Iv(sequenceNumber));
        writer.Write(content);
        writer.Write(new byte[16]);

        return writer.WrittenSpan.ToArray();
    }

    public static byte[] FinalFrame(uint sequenceNumber, byte[] content, uint? declaredLength = null)
    {
        var writer = new ArrayBufferWriter<byte>();

        WriteUInt32(writer, uint.MaxValue);
        WriteUInt32(writer, sequenceNumber);
        writer.Write(Iv(sequenceNumber));
        WriteUInt32(writer, declaredLength ?? (uint)content.Length);
        writer.Write(content);
        writer.Write(new byte[16]);

        return writer.WrittenSpan.ToArray();
    }

    public static byte[] Iv(uint sequenceNumber)
    {
        var iv = new byte[12];

        BinaryPrimitives.WriteUInt32BigEndian(iv.AsSpan(8), sequenceNumber);

        return iv;
    }

    // Message ID || Body AAD Content || Sequence Number (4) || Content Length (8)
    // https://docs.aws.amazon.com/encryption-sdk/latest/developer-guide/body-aad-reference.html
    public static byte[] SpecFrameAad(byte[] messageId, EncryptedMessageFrame frame)
    {
        var writer = new ArrayBufferWriter<byte>();

        writer.Write(messageId);
        writer.Write(frame.IsFinal ? "AWSKMSEncryptionClient Final Frame"u8 : "AWSKMSEncryptionClient Frame"u8);
        WriteUInt32(writer, frame.SequenceNumber);
        WriteUInt32(writer, 0);
        WriteUInt32(writer, (uint)frame.EncryptedContent.Length);

        return writer.WrittenSpan.ToArray();
    }

    public static EncryptedMessage Encrypt(AlgorithmSuiteId suite, byte[] rawKey, byte[] plaintext, EncryptionContext? context = null)
    {
        var key = new DataKey { RawKey = rawKey, Ciphertext = FakeEncryptedDataKey, ProviderId = "fake"u8, ProviderContext = "fake-key"u8 };

        var request = new EncryptMessageRequest {
            AlgorithmId = suite,
            EncryptionContext = context ?? [],
            Plaintext = plaintext
        };

        return suite switch {
            AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY => AES256_GCM_HKDF_SHA512_COMMIT_KEY.Default.Encrypt(key, request),
            AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY_ECDSA_P384 => AES_256_GCM_HKDF_SHA512_COMMIT_KEY_ECDSA_P384.Default.Encrypt(key, request),
            _ => throw new ArgumentOutOfRangeException(nameof(suite))
        };
    }

    public static byte[] DecryptFrame(AlgorithmSuiteId suite, byte[] rawKey, byte[] messageId, EncryptedMessageFrame frame, byte[] aad)
    {
        var derivedKey = new byte[32];

        AlgorithmSuite.Get(suite).DeriveKey(rawKey, messageId, derivedKey);

        using var aes = new AesGcm(derivedKey, 16);

        var plaintext = new byte[frame.EncryptedContent.Length];

        aes.Decrypt(frame.IV, frame.EncryptedContent, frame.AuthenticationTag, plaintext, aad);

        return plaintext;
    }

    // SEC 1 compressed point. .NET can't import these directly, so recover y from the P-384 curve equation (y^2 = x^3 - 3x + b).
    public static ECDsa ImportCompressedP384(ReadOnlySpan<byte> point)
    {
        var p = BigInteger.Pow(2, 384) - BigInteger.Pow(2, 128) - BigInteger.Pow(2, 96) + BigInteger.Pow(2, 32) - 1;
        var b = BigInteger.Parse("0b3312fa7e23ee7e4988e056be3f82d19181d9c6efe8141120314088f5013875ac656398d8a2ed19d2a85c8edd3ec2aef", NumberStyles.HexNumber);

        var x = new BigInteger(point[1..], isUnsigned: true, isBigEndian: true);
        var y = BigInteger.ModPow((((BigInteger.ModPow(x, 3, p) - (3 * x) + b) % p) + p) % p, (p + 1) / 4, p);

        if (!y.IsEven != (point[0] == 3))
        {
            y = p - y;
        }

        var yBytes = new byte[48];

        y.TryWriteBytes(yBytes.AsSpan(48 - y.GetByteCount(isUnsigned: true)), out _, isUnsigned: true, isBigEndian: true);

        return ECDsa.Create(new ECParameters {
            Curve = ECCurve.NamedCurves.nistP384,
            Q = new ECPoint { X = point[1..].ToArray(), Y = yBytes }
        });
    }

    private static void WriteUInt16(ArrayBufferWriter<byte> writer, ushort value)
    {
        BinaryPrimitives.WriteUInt16BigEndian(writer.GetSpan(2), value);
        writer.Advance(2);
    }

    private static void WriteUInt32(ArrayBufferWriter<byte> writer, uint value)
    {
        BinaryPrimitives.WriteUInt32BigEndian(writer.GetSpan(4), value);
        writer.Advance(4);
    }
}
