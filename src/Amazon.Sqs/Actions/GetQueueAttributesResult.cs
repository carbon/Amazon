using System.Text.Json.Serialization;

namespace Amazon.Sqs;

public sealed class GetQueueAttributesResult
{
    [JsonPropertyName("Attributes")]
    public Dictionary<string, string> Attributes { get; set; } = []; // init would be reset to null by the source generator when missing
}