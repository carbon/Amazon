using System.Text.Json.Serialization;

namespace Amazon.DynamoDb
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ProjectionType : byte
    {
        KEYS_ONLY,
        INCLUDE,
        ALL,
    };
}
