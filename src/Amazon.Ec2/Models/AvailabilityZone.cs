#nullable disable

namespace Amazon.Ec2;

public sealed class AvailabilityZone
{
    public string RegionName { get; init; }

    public string ZoneName { get; init; }

    public string ZoneState { get; init; }
}
