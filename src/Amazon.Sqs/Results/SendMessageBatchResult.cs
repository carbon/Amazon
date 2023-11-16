#nullable disable

using System.Text.Json.Serialization;

namespace Amazon.Sqs;

public sealed class SendMessageBatchResult
{
    [JsonPropertyName("Failed")]
    public BatchResultErrorEntry[] Failed { get; init; }

    [JsonPropertyName("Successful")]
    public SendMessageBatchResultEntry[] Successful { get; init; }
}
