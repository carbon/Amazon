using System.Text.Json.Serialization;

namespace Amazon.DynamoDb.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TableStatus
    {
        CREATING = 1,
        UPDATING = 2,
        DELETING = 3,
        ACTIVE = 4,
        INACCESSIBLE_ENCRYPTION_CREDENTIALS = 5,
        ARCHIVING = 6,
        ARCHIVED = 7
    };
}