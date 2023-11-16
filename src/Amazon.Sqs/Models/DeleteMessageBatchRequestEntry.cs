using System.Text.Json.Serialization;

namespace Amazon.Sqs;

[method:JsonConstructor]
public readonly struct DeleteMessageBatchRequestEntry(string id, string receiptHandle)
{
    [JsonPropertyName("Id")]
    public string Id { get; } = id ?? throw new ArgumentNullException(nameof(id));

    [JsonPropertyName("ReceiptHandle")]
    public string ReceiptHandle { get; } = receiptHandle ?? throw new ArgumentNullException(nameof(receiptHandle));
}