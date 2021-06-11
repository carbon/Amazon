#nullable disable

using System;
using System.Text.Json.Serialization;

namespace Amazon.S3.Events
{
    public sealed class S3Event
    {
        [JsonPropertyName("eventVersion")]
        public string EventVersion { get; init; } // 2.0 d

        [JsonPropertyName("eventSource")]
        public string EventSource { get; init; } // aws:s3

        [JsonPropertyName("awsRegion")]
        public string AwsRegion { get; init; } // us-east-1

        [JsonPropertyName("eventTime")]
        public DateTime EventTime { get; init; } // 2017-10-31T23:52:06.033Z

        [JsonPropertyName("eventName")]
        public string EventName { get; init; } // ObjectCreated:Put

        [JsonPropertyName("userIdentity")]
        public S3UserIdentity UserIdentity { get; init; }

        [JsonPropertyName("s3")]
        public S3EventDetails S3 { get; init; }     
    }
}


// https://docs.aws.amazon.com/AmazonS3/latest/dev/notification-content-structure.html