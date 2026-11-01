using System.Text.Json.Serialization;

namespace Amazon.Sqs;

public sealed class CancelMessageMoveTaskRequest : SqsRequest
{
    public CancelMessageMoveTaskRequest(string taskHandle)
    {
        ArgumentException.ThrowIfNullOrEmpty(taskHandle);

        TaskHandle = taskHandle;
    }

    [JsonPropertyName("TaskHandle")]
    public string TaskHandle { get; }
}
