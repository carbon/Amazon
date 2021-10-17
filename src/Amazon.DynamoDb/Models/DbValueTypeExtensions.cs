namespace Amazon.DynamoDb;

internal static class DbValueTypeExtensions
{
    public static string ToQuickString(this DbValueType type) => type switch
    {
        DbValueType.B    => "B",
        DbValueType.BOOL => "BOOL",
        DbValueType.BS   => "BS",
        DbValueType.L    => "L",
        DbValueType.M    => "M",
        DbValueType.N    => "N",
        DbValueType.NS   => "NS",
        DbValueType.NULL => "NULL",
        DbValueType.S    => "S",
        DbValueType.SS   => "SS",
        _                => throw new Exception("Unexpected type:" + type.ToString()),
    };
    
}
