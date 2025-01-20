using System.Buffers;
using System.Diagnostics.CodeAnalysis;

namespace Amazon.Cryptography;

public readonly struct EncryptedDataKey : IEquatable<EncryptedDataKey>
{
    public EncryptedDataKey() { }

    [SetsRequiredMembers]
    public EncryptedDataKey(Utf8String providerId, Utf8String providerInfo, byte[] cipherText)
    {
        ProviderId = providerId;
        ProviderInfo = providerInfo;
        Ciphertext = cipherText;
    }

    public required Utf8String ProviderId { get; init; }

    // used to identify the key
    public required Utf8String ProviderInfo { get; init; }

    public required byte[] Ciphertext { get; init; }

    public static EncryptedDataKey Read(ref BufferReader reader)
    {
        return new EncryptedDataKey {
            ProviderId = reader.ReadUtf8String(),
            ProviderInfo = reader.ReadUtf8String(),
            Ciphertext = reader.ReadBytes().ToArray()
        };
    }

    public bool Equals(EncryptedDataKey other)
    {
        return ProviderId.Equals(other.ProviderId)
            && ProviderInfo.Equals(other.ProviderInfo)
            && Ciphertext.AsSpan().SequenceEqual(other.Ciphertext.AsSpan());
    }

    public void WriteTo(IBufferWriter<byte> writer)
    {
        writer.WriteUtf8Bytes(ProviderId);
        writer.WriteUtf8Bytes(ProviderInfo);
        writer.WriteUInt16((ushort)Ciphertext.Length);
        writer.Write(Ciphertext);
    }
}