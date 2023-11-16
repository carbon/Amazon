using System.Text.Json.Serialization;

namespace Amazon.DynamoDb;

[method: JsonConstructor]
public sealed class CountResult(int count)
{
    [JsonPropertyName("Count")]
    public int Count { get; } = count;
}