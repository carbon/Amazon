using System.Text.Json.Serialization;

namespace Amazon.DynamoDb.Models;

[JsonConverter(typeof(JsonStringEnumConverter<KeyType>))]
public enum KeyType
{
    HASH = 1,
    RANGE = 2
};