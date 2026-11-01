using System.Text.Json.Serialization;

namespace Amazon.Sqs;

public sealed class ListQueuesRequest : SqsRequest
{
    /// <param name="queueNamePrefix">Only return queues whose name begins with this value</param>
    /// <param name="maxResults">The page size, from 1 to 1,000. Must be set to receive a NextToken</param>
    /// <param name="nextToken">The NextToken of the previous page</param>
    public ListQueuesRequest(string? queueNamePrefix = null, int? maxResults = null, string? nextToken = null)
    {
        if (maxResults is <= 0 or > 1000)
        {
            throw new ArgumentOutOfRangeException(nameof(maxResults), maxResults, "Must be between 1 and 1,000");
        }

        QueueNamePrefix = queueNamePrefix;
        MaxResults = maxResults;
        NextToken = nextToken;
    }

    [JsonPropertyName("MaxResults")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? MaxResults { get; }

    [JsonPropertyName("NextToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? NextToken { get; }

    [JsonPropertyName("QueueNamePrefix")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? QueueNamePrefix { get; }
}
