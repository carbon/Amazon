using System.Globalization;
using System.Text.Json.Serialization;

namespace Amazon.Sqs;

public sealed class SendMessageBatchRequest : SqsRequest
{
    public SendMessageBatchRequest(string queueUrl, IReadOnlyList<string> messages)
    {
        ArgumentException.ThrowIfNullOrEmpty(queueUrl);
        ArgumentNullException.ThrowIfNull(messages);

        if (messages.Count > 10)
            throw new ArgumentException("Must be 10 or fewer", nameof(messages));

        // Max payload = 256KB (262,144 bytes)

        var entries = new SendMessageBatchRequestEntry[messages.Count];

        for (int i = 0; i < messages.Count; i++)
        {
            int number = i + 1;

            string message = messages[i];

            entries[i] = new SendMessageBatchRequestEntry(
                id          : number.ToString(CultureInfo.InvariantCulture),
                messageBody : message
            );
        }

        Entries = entries;
        QueueUrl = queueUrl;
    }

    public SendMessageBatchRequest(string queueUrl, SendMessageBatchRequestEntry[] entries)
    {
        ArgumentException.ThrowIfNullOrEmpty(queueUrl);
        ArgumentNullException.ThrowIfNull(entries);

        if (entries.Length > 10)
            throw new ArgumentException("Must be 10 or fewer", nameof(entries));

        Entries = entries;
        QueueUrl = queueUrl;
    }

    [JsonPropertyName("QueueUrl")]
    public string QueueUrl { get; }

    [JsonPropertyName("Entries")]
    public SendMessageBatchRequestEntry[] Entries { get; }
}
