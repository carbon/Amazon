namespace Amazon.CloudWatch;

public static class ComparisonOperator
{
    public const string GreaterThanOrEqualToThreshold = "GreaterThanOrEqualToThreshold";
    public const string GreaterThanThreshold = "GreaterThanThreshold";
    public const string LessThanThreshold = "LessThanThreshold";
    public const string LessThanOrEqualToThreshold = "LessThanOrEqualToThreshold";

    // Anomaly detection alarms only
    public const string LessThanLowerOrGreaterThanUpperThreshold = "LessThanLowerOrGreaterThanUpperThreshold";
    public const string LessThanLowerThreshold = "LessThanLowerThreshold";
    public const string GreaterThanUpperThreshold = "GreaterThanUpperThreshold";
}
