using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Amazon.Sqs;

public sealed class ChangeMessageVisibilityBatchRequestEntry(
    string id,
    string receiptHandle,
    int visibilityTimeout)
{
    [JsonPropertyName("Id")]
    public string Id { get; } = id ?? throw new ArgumentNullException(nameof(id));

    [JsonPropertyName("ReceiptHandle")]
    public string ReceiptHandle { get; } = receiptHandle ?? throw new ArgumentNullException(nameof(receiptHandle));

    [Range(0, 43_200)]
    [JsonPropertyName("VisibilityTimeout")]
    public int VisibilityTimeout { get; } = visibilityTimeout;
}