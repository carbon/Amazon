using System.Text.Json.Serialization;

namespace Amazon.Sqs;

public sealed class ListMessageMoveTasksResult
{
    [JsonPropertyName("Results")]
    public ListMessageMoveTasksResultEntry[] Results { get; set; } = []; // init would be reset to null by the source generator when missing
}
