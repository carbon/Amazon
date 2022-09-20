using System.IO;
using System.Security.Cryptography;

namespace Amazon.S3;

internal static class StreamHelper
{
    public static byte[] ComputeSHA256(Stream stream)
    {
#if NET7_0_OR_GREATER
        byte[] hash = SHA256.HashData(stream);
#else
        using SHA256 sha = SHA256.Create();

        byte[] hash = sha.ComputeHash(stream);
#endif
        stream.Position = 0;

        return hash;
    }
}