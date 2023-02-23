using System.IO;
using System.Security.Cryptography;

namespace Amazon.S3;

internal static class StreamHelper
{
    public static byte[] ComputeSHA256(Stream stream)
    {
        byte[] hash = SHA256.HashData(stream);

        stream.Position = 0;

        return hash;
    }
}