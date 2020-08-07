using System.Text.Json.Serialization;

namespace Amazon.DynamoDb
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum KeyType
    {
        HASH = 1,
        RANGE = 2
    };
}