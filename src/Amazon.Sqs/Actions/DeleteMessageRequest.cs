using System.Text.Json.Serialization;

namespace Amazon.Sqs;

public sealed class DeleteMessageRequest : SqsRequest
{
    public DeleteMessageRequest(string queueUrl, string receiptHandle)
    {
        ArgumentException.ThrowIfNullOrEmpty(queueUrl);
        ArgumentException.ThrowIfNullOrEmpty(receiptHandle);

        QueueUrl = queueUrl;
        ReceiptHandle = receiptHandle;
    }

    [JsonPropertyName("QueueUrl")]
    public string QueueUrl { get; set; }

    [JsonPropertyName("ReceiptHandle")]
    public string ReceiptHandle { get; set; }
}