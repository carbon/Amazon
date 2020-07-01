#nullable disable

using System;
using System.Text.Json.Serialization;

namespace Amazon.S3.Events
{
    public sealed class S3Event
    {
        [JsonPropertyName("eventVersion")]
        public string EventVersion { get; set; } // 2.0 d

        [JsonPropertyName("eventSource")]
        public string EventSource { get; set; } // aws:s3

        [JsonPropertyName("awsRegion")]
        public string AwsRegion { get; set; } // us-east-1

        [JsonPropertyName("eventTime")]
        public DateTime EventTime { get; set; } // 2017-10-31T23:52:06.033Z

        [JsonPropertyName("eventName")]
        public string EventName { get; set; } // ObjectCreated:Put

        [JsonPropertyName("userIdentity")]
        public S3UserIdentity UserIdentity { get; set; }

        [JsonPropertyName("s3")]
        public S3EventDetails S3 { get; set; }     
    }
}


// https://docs.aws.amazon.com/AmazonS3/latest/dev/notification-content-structure.html