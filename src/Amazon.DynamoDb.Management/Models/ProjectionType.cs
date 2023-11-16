using System.Text.Json.Serialization;

namespace Amazon.DynamoDb.Models;

[JsonConverter(typeof(JsonStringEnumConverter<ProjectionType>))]
public enum ProjectionType
{
    KEYS_ONLY = 1,
    INCLUDE = 2,
    ALL = 3
}
