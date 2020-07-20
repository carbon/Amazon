using System.Text.Json.Serialization;

namespace Amazon.DynamoDb
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ReplicaStatus
    {
        CREATING,
        UPDATING,
        DELETING,
        ACTIVE,
    };
}
