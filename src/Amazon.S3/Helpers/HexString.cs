using System;
using System.Runtime.CompilerServices;

namespace Amazon.Helpers
{
    internal static class HexString
    {
        // Based on: http://stackoverflow.com/questions/623104/byte-to-hex-string/3974535#3974535

        public static string FromBytes(this byte[] bytes)
        {
            var buffer = new char[bytes.Length * 2];

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

        // COPYRIGHT: https://stackoverflow.com/a/17923942
        public static byte[] ToBytes(ReadOnlySpan<char> hexString)
        {
#if NET5_0
            return Convert.FromHexString(hexString);
#else
            byte[] bytes = new byte[hexString.Length / 2];

            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = ToByte(hexString[i * 2], hexString[i * 2 + 1]);
            }

            return bytes;
#endif
        }

#if !NET5_0

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static byte ToByte(char a, char b)
        {
            int hi = a - 65;

            hi = hi + 10 + ((hi >> 31) & 7);

            int lo = b - 65;

            lo = lo + 10 + ((lo >> 31) & 7) & 0x0f;

            return (byte)(lo | hi << 4);
        }

#endif
    }
}