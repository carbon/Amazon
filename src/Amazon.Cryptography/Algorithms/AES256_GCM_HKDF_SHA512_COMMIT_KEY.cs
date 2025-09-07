using System.Buffers.Binary;
using System.Security.Cryptography;

namespace Amazon.Cryptography.Algorithms;

internal sealed class AES256_GCM_HKDF_SHA512_COMMIT_KEY : AlgorithmSuite
{
    public static readonly AES256_GCM_HKDF_SHA512_COMMIT_KEY Default = new();

    public override AlgorithmSuiteId AlgorithmId => AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY;

    public override byte MessageFormatVersion => 0x02;

    public override KeySize DataKeyLength => KeySize.FromBitCount(256);

    public override int AlgorithmSuiteDataLengthBytes => 32;

    public override KeyDerivationAlgorithmType? KeyDerivationAlgorithm => KeyDerivationAlgorithmType.HKDF_SHA512;

    public EncryptedMessage Encrypt(DataKey key, EncryptMessageRequest request)
    {
        if (key.RawKey.Length != 32)
        {
            throw new ArgumentException("Key must be 32 bytes (256 bits)", nameof(key));
        }

        var messageId = RandomNumberGenerator.GetBytes(32); // 256 bits

        // Derive a key using HKDF
        Span<byte> derivedKey = stackalloc byte[32];

        DeriveKey(key.RawKey, messageId, derivedKey);

        using var aes = new AesGcm(derivedKey, 16);

        Span<byte> iv = stackalloc byte[12]; // 12-byte IV for AES-GCM

        Span<byte> tag = stackalloc byte[16]; // 16-byte authentication tag

        tag.Clear();

        byte[] associatedData = request.EncryptionContext.Serialize();

        (long chuckCount, long r) = Math.DivRem(request.Plaintext.Length, request.FrameLength);

        if (r > 0) chuckCount++;

        var frames = new List<EncryptedMessageFrame>((int)chuckCount);
        uint sequenceNumber = 1;

        int offset = 0;
        int frameLength = (int)request.FrameLength;
        ReadOnlySpan<byte> plaintext = request.Plaintext;

        while (offset < plaintext.Length)
        {
            var chunk = plaintext.Slice(offset, Math.Min(frameLength, plaintext.Length - offset));

            SetIV(sequenceNumber, iv);

            byte[] ciphertext = new byte[chunk.Length];
            aes.Encrypt(iv, chunk, ciphertext, tag, associatedData);

            var frame = new EncryptedMessageFrame {
                IV = iv.ToArray(),
                EncryptedContent = ciphertext,
                SequenceNumber = sequenceNumber,
                AuthenticationTag = tag.ToArray(),
                IsFinal = sequenceNumber == chuckCount,
            };

            sequenceNumber++;
            offset += chunk.Length;

            frames.Add(frame);
        }

        return new EncryptedMessage {
            Header = new EncryptedMessageHeader {
                AlgorithmId = AlgorithmId,
                AlgorithmSuiteData = messageId,
                MessageId = messageId,
                ContentType = 2, // framed
                EncryptionContext = request.EncryptionContext,
                AuthenticationTag = "a"u8.ToArray(),
                EncryptedDataKeys = [
                    new EncryptedDataKey(key.ProviderId, key.ProviderContext, [])
                ],
                FrameLength = request.FrameLength,
                Version = 2
            },
            Frames = frames
        };
    }

    public override void DeriveKey(ReadOnlySpan<byte> dataKey, ReadOnlySpan<byte> messageId, Span<byte> derivedKey)
    {
        if (messageId.Length != 32)
        {
            throw new ArgumentException("Must be 32 bytes (256 bits)", nameof(dataKey));
        }

        if (messageId.Length != 32)
        {
            throw new CryptographicException("Must be 32 bytes (256 bits)");
        }

        if (derivedKey.Length != 32)
        {
            throw new ArgumentException("Must be 32 bytes (256 bits)", nameof(derivedKey));
        }

        Span<byte> inputInfo = stackalloc byte[
            2 + // algorithmId 
            9   // key label
        ];

        BinaryPrimitives.WriteUInt16BigEndian(inputInfo, (ushort)AlgorithmId);
        "DERIVEKEY"u8.CopyTo(inputInfo[2..]);

        HKDF.DeriveKey(HashAlgorithmName.SHA512, dataKey, derivedKey, salt: messageId, inputInfo);
    }

    public void CalculateCommitmentKey(ReadOnlySpan<byte> dataKey, ReadOnlySpan<byte> messageId, Span<byte> commitmentKey)
    {
        HKDF.DeriveKey(HashAlgorithmName.SHA512, dataKey, commitmentKey, salt: messageId, info: "COMMITKEY"u8);
    }

    private static void SetIV(uint sequenceNumber, Span<byte> iv)
    {
        iv.Clear();

        BinaryPrimitives.WriteUInt32BigEndian(iv[8..], sequenceNumber);
    }
}