using System.IO;
using System.Security.Cryptography;

namespace Amazon.S3;

internal static class StreamHelper
{
    public static byte[] ComputeSHA256(Stream stream)
    {
        using SHA256 sha = SHA256.Create();

        byte[] data = sha.ComputeHash(stream);

        stream.Position = 0;

        return data;
    }
}