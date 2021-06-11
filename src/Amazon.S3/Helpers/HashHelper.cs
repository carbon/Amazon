using System.Security.Cryptography;
using System.Text;

namespace Amazon.S3
{
    internal static class HashHelper
    {
        public static byte[] ComputeMD5Hash(string text)
        {
            return MD5.HashData(Encoding.UTF8.GetBytes(text));
        }
    }
}