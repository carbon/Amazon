using System.Buffers;
using System.Security.Cryptography;
using System.Text;

namespace Amazon.S3;

internal static class HashHelper
{
    public static byte[] ComputeMD5Hash(ReadOnlySpan<char> text)
    {
        byte[] utf8Buffer = ArrayPool<byte>.Shared.Rent(text.Length * 4);

        int bufferLength = Encoding.UTF8.GetBytes(text, utf8Buffer);

        var result = MD5.HashData(utf8Buffer.AsSpan(0, bufferLength));

        ArrayPool<byte>.Shared.Return(utf8Buffer);

        return result;
    }
}