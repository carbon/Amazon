using System.Text.Json.Serialization;

namespace Amazon.DynamoDb.Models;

[JsonConverter(typeof(JsonStringEnumConverter<ReplicaStatus>))]
public enum ReplicaStatus
{
    CREATING = 1,
    UPDATING = 2,
    DELETING = 3,
    ACTIVE   = 4
}