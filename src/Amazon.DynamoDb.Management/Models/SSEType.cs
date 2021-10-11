using System.Text.Json.Serialization;

namespace Amazon.DynamoDb.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SseType
{
    AES256 = 1,
    KMS = 2
}