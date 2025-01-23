using System.Text.Json.Serialization;

namespace Amazon.DynamoDb;

public sealed class CountResult
{
    [JsonPropertyName("Count")]
    public int Count { get; init; }
}