#nullable disable

using System.Text.Json.Serialization;

namespace Amazon.S3.Events;

public sealed class S3Event
{
    [JsonPropertyName("eventVersion")]
    public required string EventVersion { get; init; } // 2.0 | 2.1 | 2.2

    [JsonPropertyName("eventSource")]
    public required string EventSource { get; init; } // aws:s3

    [JsonPropertyName("awsRegion")]
    public required string AwsRegion { get; init; } // us-east-1

    [JsonPropertyName("eventTime")]
    public required DateTime EventTime { get; init; } // 2017-10-31T23:52:06.033Z

    [JsonPropertyName("eventName")]
    public required string EventName { get; init; } // ObjectCreated:Put

    [JsonPropertyName("userIdentity")]
    public S3UserIdentity UserIdentity { get; init; }

    [JsonPropertyName("s3")]
    public S3EventDetails S3 { get; init; }

    // glacierEventData
}

// https://docs.aws.amazon.com/AmazonS3/latest/dev/notification-content-structure.html