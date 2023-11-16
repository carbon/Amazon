using System.Text.Json.Serialization;

namespace Amazon.Sqs;

public sealed class SendMessageBatchResultEntry
{
    [JsonPropertyName("Id")]
    public required string Id { get; init; }

    [JsonPropertyName("MessageId")]
    public required string MessageId { get; init; }

    [JsonPropertyName("MD5OfMessageBody")]
    public required string MD5OfMessageBody { get; init; }

    [JsonPropertyName("SequenceNumber")]
    public string? SequenceNumber { get; init; }
}