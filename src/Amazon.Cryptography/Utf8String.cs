using System.Text;
using System.Text.Json.Serialization;

namespace Amazon.Cryptography;

[JsonConverter(typeof(Utf8StringConverter))]
public readonly struct Utf8String(byte[] data) : IEquatable<Utf8String>
{
    private readonly byte[] _data = data;

    public ReadOnlySpan<byte> Utf8Bytes => _data;

    public int Length => _data.Length;

    public ReadOnlySpan<byte> Value => _data;

    public void CopyTo(Span<byte> destination)
    {
        _data.AsSpan().CopyTo(destination);
    }

    public bool Equals(Utf8String other)
    {
        return Utf8Bytes.SequenceEqual(other.Utf8Bytes);
    }

    public ReadOnlySpan<byte> Slice(int start) => _data.AsSpan(start);

    public override string ToString()
    {
        return Encoding.UTF8.GetString(_data);
    }

    public static implicit operator Utf8String(byte[] utf8Bytes)
    {
        return new Utf8String(utf8Bytes);
    }

    public static implicit operator Utf8String(ReadOnlySpan<byte> utf8Bytes)
    {
        return new Utf8String(utf8Bytes.ToArray());
    }

    public static implicit operator string(Utf8String instance)
    {
        return instance.ToString();
    }

    public override bool Equals(object? obj)
    {
        return obj is Utf8String other && Equals(other);
    }

    public override int GetHashCode()
    {
        var result = new HashCode();

        result.AddBytes(_data);

        return result.ToHashCode();
    }
}
