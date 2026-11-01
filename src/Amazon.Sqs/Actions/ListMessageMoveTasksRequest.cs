using System.Text.Json.Serialization;

namespace Amazon.Sqs;

public sealed class ListMessageMoveTasksRequest : SqsRequest
{
    /// <param name="sourceArn">The ARN of the queue whose tasks to list</param>
    /// <param name="maxResults">The number of tasks, up to 10. Defaults to 1, the most recent</param>
    public ListMessageMoveTasksRequest(string sourceArn, int? maxResults = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(sourceArn);

        if (maxResults is <= 0 or > 10)
        {
            throw new ArgumentOutOfRangeException(nameof(maxResults), maxResults, "Must be between 1 and 10");
        }

        SourceArn = sourceArn;
        MaxResults = maxResults;
    }

    [JsonPropertyName("MaxResults")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? MaxResults { get; }

    [JsonPropertyName("SourceArn")]
    public string SourceArn { get; }
}
