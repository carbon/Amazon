using System.Text.Json.Serialization;

namespace Amazon.DynamoDb
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum AttributeType : byte
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
        S,
    }
}