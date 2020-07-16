using System.Text.Json.Serialization;

namespace Amazon.DynamoDb
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ReplicaStatus : byte
    {
        CREATING,
        UPDATING,
        DELETING,
        ACTIVE,
    };
}
