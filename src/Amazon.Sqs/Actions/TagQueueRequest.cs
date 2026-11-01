using System.Text.Json.Serialization;

namespace Amazon.Sqs;

public sealed class TagQueueRequest : SqsRequest
{
    public TagQueueRequest(string queueUrl, Dictionary<string, string> tags)
    {
        ArgumentException.ThrowIfNullOrEmpty(queueUrl);
        ArgumentNullException.ThrowIfNull(tags);

        QueueUrl = queueUrl;
        Tags = tags;
    }

    [JsonPropertyName("QueueUrl")]
    public string QueueUrl { get; }

    [JsonPropertyName("Tags")]
    public Dictionary<string, string> Tags { get; }
}
