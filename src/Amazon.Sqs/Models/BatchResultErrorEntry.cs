using System.Text.Json.Serialization;

namespace Amazon.Sqs;

[method:JsonConstructor]
public sealed class BatchResultErrorEntry(
    string code,
    string id,
    bool senderFault,
    string? message)
{
    [JsonPropertyName("Code")]
    public string Code { get; } = code;

    [JsonPropertyName("Id")]
    public string Id { get; } = id;

    [JsonPropertyName("SenderFault")]
    public bool SenderFault { get; } = senderFault;

    [JsonPropertyName("Message")]
    public string? Message { get; } = message;
}