using System.Text.Json.Serialization;

namespace Amazon.Sqs;

public sealed class ListDeadLetterSourceQueuesResult
{
    [JsonPropertyName("NextToken")]
    public string? NextToken { get; init; }

    // Lower camel case, unlike ListQueues
    [JsonPropertyName("queueUrls")]
    public string[] QueueUrls { get; set; } = []; // init would be reset to null by the source generator when missing
}
