using System.Buffers;
using System.Buffers.Binary;
using System.Buffers.Text;
using System.Security.Cryptography;
using System.Text.Unicode;

namespace Amazon.Cryptography.Algorithms;

internal abstract class CommittingAlgorithmSuite : AlgorithmSuite
{
    private static ReadOnlySpan<byte> PublicKeyContextKey => "aws-crypto-public-key"u8;

    private static ReadOnlySpan<byte> ReservedContextKeyPrefix => "aws-crypto-"u8;

    public override byte MessageFormatVersion => 0x02;

    public override KeySize DataKeyLength => KeySize.FromBitCount(256);

    public override int AlgorithmSuiteDataLengthBytes => 32;

    public override KeyDerivationAlgorithmType? KeyDerivationAlgorithm => KeyDerivationAlgorithmType.HKDF_SHA512;

    public EncryptedMessage Encrypt(DataKey key, EncryptMessageRequest request)
    {
        if (key.RawKey is not { Length: 32 })
        {
            throw new ArgumentException("Key must be 32 bytes (256 bits)", nameof(key));
        }

        if (key.Ciphertext is not { Length: > 0 })
        {
            throw new ArgumentException("Key must include the encrypted data key", nameof(key));
        }

        if (request.AlgorithmId != AlgorithmId)
        {
            throw new ArgumentException($"Expected algorithm {AlgorithmId}. Was {request.AlgorithmId}", nameof(request));
        }

        foreach (var entry in request.EncryptionContext)
        {
            if (entry.Key.Utf8Bytes.StartsWith(ReservedContextKeyPrefix))
            {
                throw new ArgumentException($"Encryption context key '{entry.Key}' uses the reserved aws-crypto- prefix", nameof(request));
            }

            if (!Utf8.IsValid(entry.Key.Utf8Bytes) || !Utf8.IsValid(entry.Value.Utf8Bytes))
            {
                throw new ArgumentException("Encryption context keys and values must be valid UTF-8", nameof(request));
            }
        }

        using ECDsa? signingKey = SignatureAlgorithm is null ? null : ECDsa.Create(ECCurve.NamedCurves.nistP384);

        EncryptionContext encryptionContext = signingKey is null
            ? request.EncryptionContext
            : WithPublicKey(request.EncryptionContext, signingKey);

        // Validates that the context fits in the header before any work is done
        encryptionContext.GetLength();

        var messageId = RandomNumberGenerator.GetBytes(32); // 256 bits
        var commitmentKey = new byte[32];

        CalculateCommitmentKey(key.RawKey, messageId, commitmentKey);

        // Derive a key using HKDF
        Span<byte> derivedKey = stackalloc byte[32];

        try
        {
            DeriveKey(key.RawKey, messageId, derivedKey);

            using var aes = new AesGcm(derivedKey, 16);

            var header = new EncryptedMessageHeader {
                AlgorithmId = AlgorithmId,
                AlgorithmSuiteData = commitmentKey,
                MessageId = messageId,
                ContentType = 2, // framed
                EncryptionContext = encryptionContext,
                EncryptedDataKeys = [
                    new EncryptedDataKey(key.ProviderId, key.ProviderContext, key.Ciphertext)
                ],
                FrameLength = request.FrameLength,
                Version = 2
            };

            var message = new EncryptedMessage {
                Header = header with { AuthenticationTag = CalculateHeaderTag(aes, header) },
                Frames = EncryptFrames(aes, messageId, request)
            };

            return signingKey is null ? message : Sign(message, signingKey);
        }
        finally
        {
            CryptographicOperations.ZeroMemory(derivedKey);
        }
    }

    public override void DeriveKey(ReadOnlySpan<byte> dataKey, ReadOnlySpan<byte> messageId, Span<byte> derivedKey)
    {
        ValidateKeyInputs(dataKey, messageId);

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
        ValidateKeyInputs(dataKey, messageId);

        if (commitmentKey.Length != 32)
        {
            throw new ArgumentException("Must be 32 bytes (256 bits)", nameof(commitmentKey));
        }

        HKDF.DeriveKey(HashAlgorithmName.SHA512, dataKey, commitmentKey, salt: messageId, info: "COMMITKEY"u8);
    }

    private static void ValidateKeyInputs(ReadOnlySpan<byte> dataKey, ReadOnlySpan<byte> messageId)
    {
        if (dataKey.Length != 32)
        {
            throw new ArgumentException("Must be 32 bytes (256 bits)", nameof(dataKey));
        }

        if (messageId.Length != 32)
        {
            throw new ArgumentException("Must be 32 bytes (256 bits)", nameof(messageId));
        }
    }

    private static byte[] CalculateHeaderTag(AesGcm aes, in EncryptedMessageHeader header)
    {
        var writer = new ArrayBufferWriter<byte>();

        header.WriteBodyTo(writer);

        Span<byte> iv = stackalloc byte[12];

        iv.Clear();

        var tag = new byte[16];

        aes.Encrypt(iv, [], [], tag, writer.WrittenSpan);

        return tag;
    }

    private static List<EncryptedMessageFrame> EncryptFrames(AesGcm aes, byte[] messageId, EncryptMessageRequest request)
    {
        ReadOnlySpan<byte> plaintext = request.Plaintext;

        int frameLength = (int)request.FrameLength;
        int offset = 0;
        uint sequenceNumber = 1;
        bool isFinal;

        var frames = new List<EncryptedMessageFrame>((plaintext.Length / frameLength) + 1);

        Span<byte> iv = stackalloc byte[12];
        Span<byte> tag = stackalloc byte[16]; // 16-byte authentication tag

        // An empty plaintext still produces a final frame
        do
        {
            var chunk = plaintext.Slice(offset, Math.Min(frameLength, plaintext.Length - offset));

            isFinal = offset + chunk.Length == plaintext.Length;

            SetIV(sequenceNumber, iv);

            byte[] ciphertext = new byte[chunk.Length];
            byte[] associatedData = GetFrameAad(messageId, isFinal, sequenceNumber, chunk.Length);

            aes.Encrypt(iv, chunk, ciphertext, tag, associatedData);

            frames.Add(new EncryptedMessageFrame {
                IV = iv.ToArray(),
                EncryptedContent = ciphertext,
                SequenceNumber = sequenceNumber,
                AuthenticationTag = tag.ToArray(),
                IsFinal = isFinal,
            });

            sequenceNumber++;
            offset += chunk.Length;
        }
        while (!isFinal);

        return frames;
    }

    // Message ID || Body AAD Content || Sequence Number || Content Length
    private static byte[] GetFrameAad(byte[] messageId, bool isFinal, uint sequenceNumber, long contentLength)
    {
        ReadOnlySpan<byte> content = isFinal
            ? "AWSKMSEncryptionClient Final Frame"u8
            : "AWSKMSEncryptionClient Frame"u8;

        var result = new byte[messageId.Length + content.Length + 4 + 8];

        messageId.CopyTo(result, 0);
        content.CopyTo(result.AsSpan(messageId.Length));

        int position = messageId.Length + content.Length;

        BinaryPrimitives.WriteUInt32BigEndian(result.AsSpan(position), sequenceNumber);
        BinaryPrimitives.WriteInt64BigEndian(result.AsSpan(position + 4), contentLength);

        return result;
    }

    private static EncryptionContext WithPublicKey(EncryptionContext source, ECDsa signingKey)
    {
        var result = new EncryptionContext();

        foreach (var entry in source)
        {
            result.Add(entry.Key, entry.Value);
        }

        result.Add(PublicKeyContextKey, GetCompressedPublicKey(signingKey));

        return result;
    }

    // SEC 1 compressed point, base64-encoded
    private static Utf8String GetCompressedPublicKey(ECDsa signingKey)
    {
        var q = signingKey.ExportParameters(includePrivateParameters: false).Q;

        Span<byte> point = stackalloc byte[1 + q.X!.Length];

        point[0] = (byte)(0x02 | (q.Y![^1] & 1));
        q.X.CopyTo(point[1..]);

        Span<byte> encoded = stackalloc byte[Base64.GetMaxEncodedToUtf8Length(point.Length)];

        Base64.EncodeToUtf8(point, encoded, out _, out int written);

        return (ReadOnlySpan<byte>)encoded[..written];
    }

    private static EncryptedMessage Sign(EncryptedMessage message, ECDsa signingKey)
    {
        var writer = new ArrayBufferWriter<byte>();

        message.WriteTo(writer);

        return new EncryptedMessage {
            Header = message.Header,
            Frames = message.Frames,
            Signature = signingKey.SignData(writer.WrittenSpan, HashAlgorithmName.SHA384, DSASignatureFormat.Rfc3279DerSequence)
        };
    }

    private static void SetIV(uint sequenceNumber, Span<byte> iv)
    {
        iv.Clear();

        BinaryPrimitives.WriteUInt32BigEndian(iv[8..], sequenceNumber);
    }
}
