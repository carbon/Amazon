using System.Text.Json.Serialization;

namespace Amazon.Sqs;

public sealed class GetQueueAttributesResult
{
    [JsonPropertyName("Attributes")]
    public required Dictionary<string, string> Attributes { get; init; }
}