#nullable disable

using System.Text.Json.Serialization;

namespace Amazon.Sqs;

public sealed class ChangeMessageVisibilityBatchResult
{
    [JsonPropertyName("Failed")]
    public ChangeMessageVisibilityBatchResultEntry[] Failed { get; init; }

    [JsonPropertyName("Successful")]
    public ChangeMessageVisibilityBatchResultEntry[] Successful { get; init; }
}
