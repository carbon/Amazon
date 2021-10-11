using System.Text.Json.Serialization;

namespace Amazon.DynamoDb.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AttributeType
{
    Unknown = 0,

    /// <summary>
    /// Binary
    /// </summary>
    B,

    /// <summary>
    /// Number
    /// </summary>
    N,

    /// <summary>
    /// String
    /// </summary>
    S
}