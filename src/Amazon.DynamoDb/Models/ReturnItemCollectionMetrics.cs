using System.Text.Json.Serialization;

namespace Amazon.DynamoDb;

[JsonConverter(typeof(JsonStringEnumConverter<ReturnItemCollectionMetrics>))]
public enum ReturnItemCollectionMetrics
{
    SIZE = 1,
    NONE = 2
}
