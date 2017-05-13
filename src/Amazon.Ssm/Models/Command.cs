using Carbon.Json;

namespace Amazon.Ssm
{
    public class Command
    {
        public string CommandId { get; set; }

        public string Comment { get; set; }

        public int CompletedCount { get; set; }

        public int RrrorCount { get; set; }

        public string DocumentName { get; set; }

        public int ExpiresAfter { get; set; }

        public string[] InstanceIds { get; set; }

        // Can be a number or %
        public string MaxConcurrency { get; set; }


        public string MaxErrors { get; set; }

        public NotificationConfig NotificationConfig { get; set; }

        public string OutputS3BucketName { get; set; }

        public string OutputS3BucketKeyPrefix { get; set; }

        public JsonObject Parameters { get; set; }

        public int RequestedDateTime { get; set; }

        public string ServiceRole { get; set; }

        public string Status { get; set; }

        public string StatusDetails { get; set; }

        public int TargetCount { get; set; }

        public Target[] Targets { get; set; }
    }

    public class Target
    {
        public string Key { get; set; }

        public string[] Values { get; set; }
    }

    public class NotificationConfig
    {
        public string NotificationArn { get; set; }

        public string NotificationEvents { get; set; }

        public string NotificationType { get; set; }
    }
}
