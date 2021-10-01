#nullable disable

using System.ComponentModel.DataAnnotations;

namespace Amazon.Elb;

public sealed class ModifyTargetGroupRequest : IElbRequest
{
    public string Action => "ModifyTargetGroup";

    public int? HealthCheckIntervalSeconds { get; init; }

    public string HealthCheckPath { get; init; }

    public int? HealthCheckPort { get; init; }

    public string HealthCheckProtocol { get; init; }

    public int? HealthCheckTimeoutSeconds { get; init; }

    public int? HealthyThresholdCount { get; init; }

    public Matcher Matcher { get; init; }

    [Required]
    public string TargetGroupArn { get; init; }

    public int? UnhealthyThresholdCount { get; init; }
}