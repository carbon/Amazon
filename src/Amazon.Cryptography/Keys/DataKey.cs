namespace Amazon.Cryptography;

public readonly ref struct DataKey
{
    public byte[] RawKey { get; init; }

    public Utf8String ProviderId { get; init; }

    public Utf8String ProviderContext { get; init; }
}