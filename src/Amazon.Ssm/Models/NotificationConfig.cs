#nullable disable

namespace Amazon.Ssm;

public sealed class NotificationConfig
{
    public string NotificationArn { get; set; }

    // All | InProgress | Success | TimedOut | Cancelled | Failed
    public string[] NotificationEvents { get; set; }

    public string NotificationType { get; set; }
}
