using System.Text.Json.Serialization;

namespace Amazon.S3.Events;

public sealed class S3Notification
{
    [JsonPropertyName("Records")]
    public required S3Event[] Records { get; init; }
}

// https://docs.aws.amazon.com/AmazonS3/latest/dev/notification-content-structure.html