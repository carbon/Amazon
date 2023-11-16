using System.Text.Json.Serialization;

namespace Amazon.Sqs;

public sealed class SendMessageResult
{
    [JsonPropertyName("MD5OfMessageBody")]
    public required string MD5OfMessageBody { get; init; }

    [JsonPropertyName("MD5OfMessageAttributes")]
    public string? MD5OfMessageAttributes { get; init; }

    [JsonPropertyName("MessageId")]
    public required string MessageId { get; init; }

    [JsonPropertyName("SequenceNumber")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? SequenceNumber { get; init; }
}