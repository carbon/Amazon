using System.IO;
using System.Security.Cryptography;
using System.Threading;

namespace Amazon.S3;

internal static class StreamHelper
{
    public static byte[] ComputeSHA256(Stream stream)
    {
        byte[] hash = SHA256.HashData(stream);

        stream.Position = 0;

        return hash;
    }

    public static async ValueTask<byte[]> ComputeSHA256Async(Stream stream, CancellationToken cancellationToken = default)
    {
        byte[] hash = await SHA256.HashDataAsync(stream, cancellationToken).ConfigureAwait(false);

        stream.Position = 0;

        return hash;
    }

    public static string ComputeSHA256Hex(Stream stream)
    {
        Span<byte> hash = stackalloc byte[SHA256.HashSizeInBytes];

        SHA256.HashData(stream, hash);

        stream.Position = 0;

        return Convert.ToHexStringLower(hash);
    }
}