using System.Text.Json.Serialization;

namespace Amazon.Sqs;

public sealed class ReceiveMessageResult
{
    [JsonPropertyName("Messages")]
    public required SqsMessage[] Messages { get; init; }
}