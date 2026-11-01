namespace Amazon.Cryptography;

public readonly ref struct DataKey
{
    public byte[] RawKey { get; init; }

    // The data key encrypted by the key provider
    public byte[] Ciphertext { get; init; }

    public Utf8String ProviderId { get; init; }

    public Utf8String ProviderContext { get; init; }
}