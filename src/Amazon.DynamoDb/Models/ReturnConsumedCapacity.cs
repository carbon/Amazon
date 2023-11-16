using System.Text.Json.Serialization;

namespace Amazon.DynamoDb;

[JsonConverter(typeof(JsonStringEnumConverter<ReturnConsumedCapacity>))]
public enum ReturnConsumedCapacity
{
    INDEXES = 1,
    TOTAL = 2,
    NONE = 3
}