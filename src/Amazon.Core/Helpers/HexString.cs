using System.Runtime.CompilerServices;

namespace Amazon.Helpers;

internal static class HexString
{
    [SkipLocalsInit]
    public static void DecodeBytesTo(ReadOnlySpan<byte> bytes, Span<char> destination)
    {
        // NOTE .NET 9.0 RTM has a bug where this fails if the buffer is not the exact size
        Convert.TryToHexStringLower(bytes, destination.Slice(0, bytes.Length * 2), out _);
    }
}