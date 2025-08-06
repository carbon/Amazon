using System.Buffers;
using System.Buffers.Binary;

using Amazon.Cryptography.Buffers;

namespace Amazon.Cryptography;

public sealed class EncryptionContext : OrderedDictionary<Utf8String, Utf8String>
{
    public ushort GetLength()
    {
        if (Count is 0) return 0;

        int length = (Count * 4) + 2;

        foreach (var entry in this)
        {
            length += entry.Key.Length;
            length += entry.Value.Length;
        }

        return (ushort)length;
    }

    public byte[] ToArray()
    {
        byte[] result = new byte[GetLength()];

        WriteTo(result);

        return result;
    }

    public void WriteTo(Span<byte> destination)
    {
        if (Count is 0)
        {
            return;
        }

        int position = 0;

        BinaryPrimitives.WriteUInt16BigEndian(destination, (ushort)Count);
        position += 2;

        foreach (var entry in this)
        {
            BinaryPrimitives.WriteUInt16BigEndian(destination.Slice(position, 2), (ushort)entry.Key.Length);
            position += 2;

            entry.Key.CopyTo(destination[position..]);
            position += entry.Key.Length;

            BinaryPrimitives.WriteUInt16BigEndian(destination.Slice(position, 2), (ushort)entry.Value.Length);
            position += 2;

            entry.Value.CopyTo(destination[position..]);
            position += entry.Value.Length;
        }
    }

    public void WriteTo(IBufferWriter<byte> writer)
    {
        if (Count is 0)
        {
            return;
        }

        writer.WriteUInt16((ushort)Count);

        foreach (var entry in this)
        {
            writer.WriteUtf8Bytes(entry.Key);
            writer.WriteUtf8Bytes(entry.Value);
        }        
    }

    public static EncryptionContext Parse(ReadOnlySpan<byte> data)
    {
        if (data.IsEmpty)
        {
            return [];
        }

        var reader = new BufferReader(data);

        uint count = reader.ReadUInt16();

        var result = new EncryptionContext();

        for (int i = 0; i < count; i++)
        {
            var key = reader.ReadUtf8String();
            var value = reader.ReadUtf8String();

            result[key] = value;
        }

        return result;
    }
}