using System.Security.Cryptography;
using System.Text;

namespace Amazon.S3
{
    internal static class HashHelper
    {
        public static byte[] ComputeMD5Hash(string text)
        {
            return ComputeMD5Hash(Encoding.UTF8.GetBytes(text));
        }

        public static byte[] ComputeMD5Hash(byte[] data)
        {
#if NET5_0_OR_GREATER
            return MD5.HashData(data);
#else
            using MD5 md5 = MD5.Create();

            return md5.ComputeHash(data);
#endif
        }
    }
}