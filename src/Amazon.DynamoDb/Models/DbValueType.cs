using System.Text.Json.Serialization;

namespace Amazon.DynamoDb;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DbValueType : byte
{
    Unknown = 0,

    /// <summary>
    /// Binary
    /// </summary>
    B,

    /// <summary>
    /// BinarySet
    /// </summary>
    BS,

    /// <summary>
    /// Number
    /// </summary>
    N,

    /// <summary>
    /// String
    /// </summary>
    S,

    /// <summary>
    /// String Set
    /// </summary>
    SS,

    /// <summary>
    /// Number Set
    /// </summary>
    NS,

    /// <summary>
    /// Boolean
    /// </summary>
    BOOL,

    /// <summary>
    /// Null
    /// </summary>
    NULL,

    /// <summary>
    /// List
    /// </summary>
    L,

    /// <summary>
    /// Map
    /// </summary>
    M
}


public static class DbValueTypeExtensions
{
    public static string ToQuickString(this DbValueType type)
    {
        return type switch
        {
            DbValueType.B => "B",
            DbValueType.BOOL => "BOOL",
            DbValueType.BS => "BS",
            DbValueType.L => "L",
            DbValueType.M => "M",
            DbValueType.N => "N",
            DbValueType.NS => "NS",
            DbValueType.NULL => "NULL",
            DbValueType.S => "S",
            DbValueType.SS => "SS",
            _ => throw new Exception("Unexpected type:" + type.ToString()),
        };
    }
}
