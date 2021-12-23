#nullable disable

using System.Text.Json.Serialization;

namespace Amazon.Ses;

public sealed class SesNotificationAction
{
    // S3 | SNS
    [JsonPropertyName("type")]
    public SesActionType Type { get; init; }

#nullable enable
    [JsonPropertyName("topicArn")]
    public string? TopicArn { get; init; }

    [JsonPropertyName("functionArn")]
    public string? FunctionArn { get; init; }

    [JsonPropertyName("bucketName")]
    public string? BucketName { get; init; }

    [JsonPropertyName("objectKey")]
    public string? ObjectKey { get; init; }

}