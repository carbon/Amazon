using System;

namespace Amazon.Helpers
{
    public static class HexString
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

        public static byte[] ToBytes(string hexString)
        {
            #region Preconditions

            if (hexString == null)
                throw new ArgumentNullException(nameof(hexString));

            if (hexString.Length % 2 != 0)
                throw new ArgumentException("Must be divisible by 2");

            #endregion

            byte[] bytes = new byte[hexString.Length / 2];

            for (int i = 0; i < hexString.Length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
            }

            return bytes;
        }
    }
}