using System.Text.Json.Serialization;

namespace Amazon.Transcribe;

public sealed class JobExecutionSettings
{
    [JsonPropertyName("AllowDeferredExecution")]
    public bool? AllowDeferredExecution { get; set; }

    [JsonPropertyName("DataAccessRoleArn")]
    public string? DataAccessRoleArn { get; set; }
}