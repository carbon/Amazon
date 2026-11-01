using System.Text.Json.Serialization;

namespace Amazon.Sqs;

public sealed class ListQueueTagsResult
{
    [JsonPropertyName("Tags")]
    public Dictionary<string, string> Tags { get; set; } = []; // init would be reset to null by the source generator when missing
}
