using System;
using System.Runtime.CompilerServices;

namespace Amazon.Helpers;

internal static class HexString
{
    // Based on: http://stackoverflow.com/questions/623104/byte-to-hex-string/3974535#3974535

    [SkipLocalsInit]
    public static string FromBytes(ReadOnlySpan<byte> bytes)
    {
        Span<char> buffer = bytes.Length < 100
            ? stackalloc char[bytes.Length * 2]
            : new char[bytes.Length * 2];

        byte b;

        for (int bx = 0, cx = 0; bx < bytes.Length; ++bx, ++cx)
        {
            b = ((byte)(bytes[bx] >> 4));

            buffer[cx] = (char)(b > 9 ? b + 0x37 + 0x20 : b + 0x30);

            b = ((byte)(bytes[bx] & 0x0F));

            buffer[++cx] = (char)(b > 9 ? b + 0x37 + 0x20 : b + 0x30);
        }

        return new string(buffer);
    }

    [SkipLocalsInit]
    public static void DecodeBytesTo(ReadOnlySpan<byte> bytes, Span<char> buffer)
    {
        byte b;

        for (int bx = 0, cx = 0; bx < bytes.Length; ++bx, ++cx)
        {
            b = ((byte)(bytes[bx] >> 4));

            buffer[cx] = (char)(b > 9 ? b + 0x37 + 0x20 : b + 0x30);

            b = ((byte)(bytes[bx] & 0x0F));

            buffer[++cx] = (char)(b > 9 ? b + 0x37 + 0x20 : b + 0x30);
        }

    }
}
