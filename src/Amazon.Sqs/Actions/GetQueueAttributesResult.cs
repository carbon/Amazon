using System.Text.Json.Serialization;

namespace Amazon.Sqs;

[method: JsonConstructor]
public sealed class GetQueueAttributesResult(Dictionary<string, string> attributes)
{
    [JsonPropertyName("Attributes")]
    public Dictionary<string, string> Attributes { get; } = attributes;
}