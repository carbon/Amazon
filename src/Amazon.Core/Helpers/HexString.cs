using System;
using System.IO;

namespace Amazon.Helpers
{
    public static class HexString
    {
        // Based on: http://stackoverflow.com/questions/623104/byte-to-hex-string/3974535#3974535

        public static string FromBytes(byte[] bytes)
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

        // based on https://stackoverflow.com/questions/321370/how-can-i-convert-a-hex-string-to-a-byte-array
        public static byte[] ToBytes(this ReadOnlySpan<char> hex)
        {
            if (hex.Length % 2 == 1)
                throw new ArgumentException("The binary key cannot have an odd number of digits");

            byte[] arr = new byte[hex.Length >> 1];

            for (int i = 0; i < hex.Length >> 1; ++i)
            {
                arr[i] = (byte)((GetHexVal(hex[i << 1]) << 4) + (GetHexVal(hex[(i << 1) + 1])));
            }

            return arr;
        }

        public static int GetHexVal(char hex)
        {
            int val = (int)hex;
            //For uppercase A-F letters:
            //return val - (val < 58 ? 48 : 55);
            //For lowercase a-f letters:
            //return val - (val < 58 ? 48 : 87);
            //Or the two combined, but a bit slower:
            return val - (val < 58 ? 48 : (val < 97 ? 55 : 87));
        }
    }
}