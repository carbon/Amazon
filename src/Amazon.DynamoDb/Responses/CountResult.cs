using System.Text.Json.Serialization;

namespace Amazon.DynamoDb;

public sealed class CountResult
{
    [JsonConstructor]
    public CountResult(int count)
    {
        Count = count;
    }

    [JsonPropertyName("Count")]
    public int Count { get; }
}