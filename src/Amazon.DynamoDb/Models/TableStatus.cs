using System.Text.Json.Serialization;

namespace Amazon.DynamoDb
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TableStatus : byte
    {
        CREATING,
        UPDATING,
        DELETING,
        ACTIVE,
        INACCESSIBLE_ENCRYPTION_CREDENTIALS,
        ARCHIVING,
        ARCHIVED,
    };
}
