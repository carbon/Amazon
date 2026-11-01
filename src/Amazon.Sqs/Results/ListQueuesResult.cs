using System.Text.Json.Serialization;

namespace Amazon.Sqs;

public sealed class ListQueuesResult
{
    [JsonPropertyName("NextToken")]
    public string? NextToken { get; init; }

    [JsonPropertyName("QueueUrls")]
    public string[] QueueUrls { get; set; } = []; // init would be reset to null by the source generator when missing
}
