using System.Buffers;
using System.Buffers.Binary;

namespace Amazon.Cryptography.Buffers;

internal static class IBufferWriterExtensions
{
    public static void WriteUInt16(this IBufferWriter<byte> writer, ushort value)
    {
        BinaryPrimitives.WriteUInt16BigEndian(writer.GetSpan(2), value);
        writer.Advance(2);
    }

    public static void WriteUInt32(this IBufferWriter<byte> writer, uint value)
    {
        BinaryPrimitives.WriteUInt32BigEndian(writer.GetSpan(4), value);
        writer.Advance(4);
    }

    public static void WriteUtf8Bytes(this IBufferWriter<byte> writer, in Utf8String text)
    {
        writer.WriteUInt16((ushort)text.Length);
        writer.Write(text.Value);
    }  
}