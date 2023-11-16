#nullable disable

using System.Text.Json.Serialization;

namespace Amazon.Sqs;

public sealed class DeleteMessageBatchResult
{
    [JsonPropertyName("Failed")]
    public BatchResultErrorEntry[] Failed { get; init; }

    [JsonPropertyName("Successful")]
    public DeleteMessageBatchResultEntry[] Successful { get; init; }
}