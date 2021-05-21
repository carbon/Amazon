using System;
using System.IO;

namespace Amazon.Helpers
{
    internal static class HexString
    {
        // Based on: http://stackoverflow.com/questions/623104/byte-to-hex-string/3974535#3974535

        public static string FromBytes(byte[] bytes)
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

        public static void WriteHexStringTo(TextWriter writer, byte[] bytes)
        {
            byte b;

            for (int bx = 0, cx = 0; bx < bytes.Length; ++bx, ++cx)
            {
                b = ((byte)(bytes[bx] >> 4));

                writer.Write((char)(b > 9 ? b + 0x37 + 0x20 : b + 0x30));

                b = ((byte)(bytes[bx] & 0x0F));

                writer.Write((char)(b > 9 ? b + 0x37 + 0x20 : b + 0x30));
            }
        }
    }
}