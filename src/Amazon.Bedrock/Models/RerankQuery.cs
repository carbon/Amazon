using System.Text.Json.Serialization;

namespace Amazon.Bedrock.Models;

public sealed class RerankQuery
{
    [JsonPropertyName("textQuery")]
    public required RerankTextDocument TextQuery { get; init; }

    [JsonPropertyName("type")]
    public required RerankQueryType Type { get; init; }

    public static implicit operator RerankQuery(string text)
    {
        return new RerankQuery {
            TextQuery = text,
            Type = RerankQueryType.Text
        };
    }
}