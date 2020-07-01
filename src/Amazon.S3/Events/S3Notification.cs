#nullable disable

using System.Text.Json.Serialization;

namespace Amazon.S3.Events
{
    public sealed class S3Notification
    {
        [JsonPropertyName("Records")]
        public S3Event[] Records { get; set; }
    }

    // https://docs.aws.amazon.com/AmazonS3/latest/dev/notification-content-structure.html
}