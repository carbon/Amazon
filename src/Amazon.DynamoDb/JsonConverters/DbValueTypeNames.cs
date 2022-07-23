namespace Amazon.DynamoDb.JsonConverters;

internal static class DbValueTypeNames
{
    public static class Utf8
    {
        public static ReadOnlySpan<byte> B    => "B"u8;
        public static ReadOnlySpan<byte> BS   => "BS"u8;
        public static ReadOnlySpan<byte> N    => "N"u8;
        public static ReadOnlySpan<byte> S    => "S"u8;
        public static ReadOnlySpan<byte> SS   => "SS"u8;
        public static ReadOnlySpan<byte> NS   => "NS"u8;
        public static ReadOnlySpan<byte> L    => "L"u8;
        public static ReadOnlySpan<byte> M    => "M"u8;
        public static ReadOnlySpan<byte> BOOL => "BOOL"u8;
    }
}
