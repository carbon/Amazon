using System.Text.Json.Serialization;

namespace Amazon.DynamoDb
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ProjectionType
    {
        KEYS_ONLY = 1,
        INCLUDE = 2,
        ALL = 3
    };
}
