namespace Amazon.Route53;

public sealed class HostedZoneConfig
{
    public string? Comment { get; init; }

    public bool PrivateZone { get; init; }
}