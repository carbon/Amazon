using System.Globalization;
using System.Text.Json.Serialization;

namespace Amazon.Sqs;

public sealed class DeleteMessageBatchRequest : SqsRequest
{
    public DeleteMessageBatchRequest(string queueUrl, string[] receiptHandles)
    {
        ArgumentException.ThrowIfNullOrEmpty(queueUrl);
        ArgumentNullException.ThrowIfNull(receiptHandles);

        if (receiptHandles.Length > 10)
        {
            throw new ArgumentException("Must contain 10 or fewer items", nameof(receiptHandles));
        }

        var entries = new DeleteMessageBatchRequestEntry[receiptHandles.Length];

        for (int i = 0; i < receiptHandles.Length; i++)
        {
            entries[i] = new DeleteMessageBatchRequestEntry((i + 1).ToString(CultureInfo.InvariantCulture), receiptHandles[i]);
        }

        Entries = entries;
        QueueUrl = queueUrl;
    }

    public DeleteMessageBatchRequest(string queueUrl, DeleteMessageBatchRequestEntry[] entries)
    {
        ArgumentException.ThrowIfNullOrEmpty(queueUrl);
        ArgumentNullException.ThrowIfNull(entries);

        if (entries.Length > 10)
            throw new ArgumentException("Must contain 10 or fewer items", nameof(entries));

        // TODO: check payload length

        Entries = entries;
        QueueUrl = queueUrl;
    }

    [JsonPropertyName("Entries")]
    public DeleteMessageBatchRequestEntry[] Entries { get; set; }

    [JsonPropertyName("QueueUrl")]
    public string QueueUrl { get; set; }
}
