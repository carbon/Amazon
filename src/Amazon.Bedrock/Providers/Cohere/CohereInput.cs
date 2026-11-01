using System.Text.Json.Serialization;

namespace Cohere;

public class CohereInput
{
    [JsonPropertyName("content")]
    public required CoherePart[] Content { get; init; }
}