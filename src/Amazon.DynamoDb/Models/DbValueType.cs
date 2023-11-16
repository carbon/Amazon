using System.Text.Json.Serialization;

namespace Amazon.DynamoDb;

[JsonConverter(typeof(JsonStringEnumConverter<DbValueType>))]
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