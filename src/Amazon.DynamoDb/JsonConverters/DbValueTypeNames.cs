namespace Amazon.DynamoDb.JsonConverters;

internal static class DbValueTypeNames
{
    public static class Utf8
    {
        public static ReadOnlySpan<byte> B    => new byte[] { (byte)'B' };
        public static ReadOnlySpan<byte> BS   => new byte[] { (byte)'B', (byte)'S' };
        public static ReadOnlySpan<byte> N    => new byte[] { (byte)'N' };
        public static ReadOnlySpan<byte> S    => new byte[] { (byte)'S' };
        public static ReadOnlySpan<byte> SS   => new byte[] { (byte)'S', (byte)'S' };
        public static ReadOnlySpan<byte> NS   => new byte[] { (byte)'N', (byte)'S' };
        public static ReadOnlySpan<byte> L    => new byte[] { (byte)'L' };
        public static ReadOnlySpan<byte> M    => new byte[] { (byte)'M' };
        public static ReadOnlySpan<byte> BOOL => new byte[] { (byte)'B', (byte)'O', (byte)'O', (byte)'L' };
    }
}
