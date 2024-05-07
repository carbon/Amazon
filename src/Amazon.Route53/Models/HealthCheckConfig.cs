using System.Xml.Serialization;

namespace Amazon.Route53;

public sealed class HealthCheckConfig
{
    // HTTP | HTTPS | HTTP_STR_MATCH | HTTPS_STR_MATCH | TCP | CALCULATED | CLOUDWATCH_METRIC | RECOVERY_CONTROL
    public required string Type { get; init; }

    public string? IPAddress { get; init; }

    public int Port { get; init; }

    public string? ResourcePath { get; init; }

    public string? FullyQualifiedDomainName { get; init; }

    public int RequestInterval { get; init; }

    public int FailureThreshold { get; init; }

    public bool MeasureLatency { get; init; }

    public bool EnableSNI { get; init; }

    [XmlArray]
    [XmlArrayItem("Region")]
    public string[]? Regions { get; init; }

    public bool Inverted { get; init; }
}