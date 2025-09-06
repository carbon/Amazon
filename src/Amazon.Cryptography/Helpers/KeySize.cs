namespace Amazon.Cryptography;

public readonly struct KeySize
{
    private readonly int _byteCount;

    internal KeySize(int byteCount)
    {
        _byteCount = byteCount;
    }

    public int BitCount => _byteCount * 8;

    public int ByteCount => _byteCount;

    public static KeySize FromBitCount(int value)
    {
        return new KeySize(value / 8);
    }

    public static KeySize FromByteCount(int value)
    {
        return new KeySize(value);
    }
}