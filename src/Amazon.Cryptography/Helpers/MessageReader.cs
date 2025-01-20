using System.Buffers.Binary;

namespace Amazon.Cryptography;

public ref struct BufferReader
{
    private ReadOnlySpan<byte> _span;
    private int _position;

    public BufferReader(ReadOnlySpan<byte> span)
    {
        _span = span;
        _position = 0;
    }

    public Utf8String ReadUtf8String()
    {
        var length = ReadUInt16();
        
        return ReadBytes(length);
    }

    public byte ReadByte()
    {
        return _span[_position++];
    }

    public ushort ReadUInt16()
    {
        var value = BinaryPrimitives.ReadUInt16BigEndian(_span.Slice(_position, 2));
        _position += 2;
        return value;
    }

    public uint ReadUInt32()
    {
        var value = BinaryPrimitives.ReadUInt32BigEndian(_span.Slice(_position, 4));
        _position += 4;
        return value;
    }

    public ReadOnlySpan<byte> ReadBytes()
    {
        var length = ReadUInt16();

        return ReadBytes(length);
    }

    public ReadOnlySpan<byte> ReadBytes(int count)
    {
        var value = _span.Slice(_position, count);
        _position += count;
        return value;
    }

    public bool IsEof => _span.Length == _position;
}