using System.Buffers;
using System.Buffers.Binary;
using System.Text.Unicode;

using Amazon.Cryptography.Buffers;

namespace Amazon.Cryptography;

public sealed class EncryptionContext : OrderedDictionary<Utf8String, Utf8String>
{
    public ushort GetLength()
    {
        if (Count is 0) return 0;

        long length = (Count * 4L) + 2;

        foreach (var entry in this)
        {
            length += entry.Key.Length;
            length += entry.Value.Length;
        }

        // Also bounds the pair count and each key and value length, which are all 16-bit
        if (length > ushort.MaxValue)
        {
            throw new InvalidOperationException($"Encryption context must not exceed {ushort.MaxValue} bytes. Was {length}");
        }

        return (ushort)length;
    }

    // Serialized entries must be sorted by key, in ascending order of their UTF-8 bytes
    private KeyValuePair<Utf8String, Utf8String>[] GetSortedEntries()
    {
        var entries = this.ToArray();

        Array.Sort(entries, static (a, b) => a.Key.Utf8Bytes.SequenceCompareTo(b.Key.Utf8Bytes));

        return entries;
    }

    public byte[] Serialize()
    {
        byte[] result = new byte[GetLength()];

        Serialize(result);

        return result;
    }

    public void Serialize(Span<byte> destination)
    {
        if (Count is 0)
        {
            return;
        }

        GetLength();

        int position = 0;

        BinaryPrimitives.WriteUInt16BigEndian(destination, (ushort)Count);
        position += 2;

        foreach (var entry in GetSortedEntries())
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

    public void Serialize(IBufferWriter<byte> writer)
    {
        if (Count is 0)
        {
            return;
        }

        GetLength();

        writer.WriteUInt16((ushort)Count);

        foreach (var entry in GetSortedEntries())
        {
            writer.WriteUtf8Bytes(entry.Key);
            writer.WriteUtf8Bytes(entry.Value);
        }        
    }

    public static EncryptionContext Deserialize(ReadOnlySpan<byte> data)
    {
        if (data.IsEmpty)
        {
            return [];
        }

        var reader = new BufferReader(data);

        uint count = reader.ReadUInt16();

        if (count is 0)
        {
            throw new FormatException("Key-value pair count must be greater than 0");
        }

        var result = new EncryptionContext();

        for (int i = 0; i < count; i++)
        {
            var key = reader.ReadUtf8String();
            var value = reader.ReadUtf8String();

            if (!Utf8.IsValid(key.Utf8Bytes) || !Utf8.IsValid(value.Utf8Bytes))
            {
                throw new FormatException("Encryption context keys and values must be valid UTF-8");
            }

            // Strictly ascending keys rule out duplicates, and keep the serialized bytes identical on rewrite
            if (i > 0 && result.GetAt(i - 1).Key.Utf8Bytes.SequenceCompareTo(key.Utf8Bytes) >= 0)
            {
                throw new FormatException("Encryption context keys must be unique and sorted in ascending order");
            }

            result.Add(key, value);
        }

        if (!reader.IsEof)
        {
            throw new FormatException("Unexpected data after the encryption context");
        }

        return result;
    }
}