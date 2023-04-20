using System.Buffers;
using System.Security.Cryptography;
using System.Text;

namespace Amazon.S3;

internal static class HashHelper
{
    public static byte[] ComputeMD5Hash(ReadOnlySpan<char> text)
    {
        byte[] rentedBuffer = ArrayPool<byte>.Shared.Rent(Encoding.UTF8.GetMaxByteCount(text.Length));

        int bufferLength = Encoding.UTF8.GetBytes(text, rentedBuffer);

        var result = MD5.HashData(rentedBuffer.AsSpan(0, bufferLength));

        ArrayPool<byte>.Shared.Return(rentedBuffer);

        return result;
    }
}