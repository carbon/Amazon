namespace Amazon.Route53;

public sealed class HostedZone
{
    public required string CallerReference { get; set; }

    public required string Id { get; set; }

    public required string Name { get; set; }

    public HostedZoneConfig? Config { get; set; }

    /// <summary>
    /// If the hosted zone was created by another service, the service that created the hosted zone.
    /// </summary>
    public LinkedService? LinkedService { get; set; }

    /// <summary>
    /// The number of resource record sets in the hosted zone.
    /// </summary>
    public long ResourceRecordSetCount { get; set; }
}