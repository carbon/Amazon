#nullable disable

namespace Amazon.S3.Events
{
    public class S3Notification
    {
        public S3Event[] Records { get; set; }
    }

    // https://docs.aws.amazon.com/AmazonS3/latest/dev/notification-content-structure.html
}